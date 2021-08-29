using Nakama;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [Header("Network")]
    [SerializeField]
    private GameConnection _connection;
    private GameStateManager _stateManager;
    private IUserPresence _self;
    private IUserPresence _enemy;
    private bool IsHost;
    private int playersReadyCount = 0;

    [Header("Gameplay")]
    [SerializeField]
    private QuestionManager questionManager;
    [SerializeField]
    private bool isPlaying = false;
    private int maxRounds = 0;
    private int currentRound = 0;

    protected override void Awake()
    {
        base.Awake();
        EventHandling.EventUtils.AddListener(EventConstants.OnGameInitialized, OnGameInitialized);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Debug.Log("GameplayManager Start");

        _stateManager = new GameStateManager(_connection);
        _stateManager.OnGameInitialized += OnGameInitializedReceived;
        _stateManager.OnPlayerReady += OnPlayerReadyReceived;


        if (_connection.BattleConnection.Self != null && _connection.BattleConnection.Opponents != null)
        {
            SetInitialPlayerState();
        }

    }

    private void SetInitialPlayerState()
    {
        Debug.Log("SetInitialPlayerState " + _connection.BattleConnection.Opponents.Count());

        _self = _connection.BattleConnection.Self;

        IsHost = (_self.UserId == _connection.BattleConnection.HostId);

        foreach (var presence in _connection.BattleConnection.Opponents)
        {
            if (presence.UserId != _connection.BattleConnection.Self.UserId)
            {
                _enemy = presence;
            }
        }

        Debug.LogFormat("IsHost {0} | SelfId {1} | EnemyId {2}", IsHost, _self.UserId, _enemy.UserId);

        if (IsHost)
        {
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionsInit, GameConfigurationManager.Instance.GameConfiguration.GameRounds);
        }
    }

    private void OnGameInitialized(EventHandling.EventArgs eventArgs)
    {
        Debug.Log("OnGameInitialized");
        SendOnGameInitialized();
    }

    private void SendOnGameInitialized()
    {
        Debug.Log("SendOnGameInitialized");
        var message = new MatchMessageOnGameInitialized(GameConfigurationManager.Instance.GameConfiguration.GameRounds);
        if (IsHost)
        {
            _stateManager.SendMatchStateMessage(MatchMessageType.GameInitialized, message);
            _stateManager.SendMatchStateMessageSelf(MatchMessageType.GameInitialized, message);
        }
    }

    private void OnGameInitializedReceived(MatchMessageOnGameInitialized message)
    {
        Debug.Log("OnGameInitializedReceived");
        maxRounds = message.Rounds;
        if (!IsHost)
        {
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnQuestionsInit, maxRounds);
        }

        SendOnPlayerReady();
    }

    private void SendOnPlayerReady()
    {
        Debug.Log("SendOnPlayerReady");
        MatchMessageOnPlayerReady message = new MatchMessageOnPlayerReady(_self.UserId);
        if (IsHost)
        {
            _stateManager.SendMatchStateMessageSelf(MatchMessageType.PlayerReady, message);
        }
        else
        {
            _stateManager.SendMatchStateMessage(MatchMessageType.PlayerReady, message);
        }

    }

    private void OnPlayerReadyReceived(MatchMessageOnPlayerReady message)
    {
        Debug.Log("OnPlayerReadyReceived");
        playersReadyCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying && playersReadyCount == 2)
        {
            isPlaying = true;
            Debug.Log("IsPlaying");
        }
        else
        {
            if (currentRound < maxRounds)
            {

            }
        }
        
    }

    protected override void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnGameInitialized, OnGameInitialized);
        base.OnDestroy();
    }

    private async void OnApplicationQuit()
    {
        await _connection.Socket.CloseAsync();
    }
}
