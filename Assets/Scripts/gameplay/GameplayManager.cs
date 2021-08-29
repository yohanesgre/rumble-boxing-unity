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
    private Player player;
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private bool isPlaying = false;
    [SerializeField]
    private bool isWinner = false;
    [SerializeField]
    private bool isOver = false;
    [SerializeField]
    private bool isGameEndScreen = false;
    [SerializeField]
    private int maxRounds;
    [SerializeField]
    private int currentRound;

    protected override void Awake()
    {
        base.Awake();
        EventHandling.EventUtils.AddListener(EventConstants.OnGameInitialized, OnGameInitialized);
        EventHandling.EventUtils.AddListener(EventConstants.OnGameRoundUpdate, OnGameRoundUpdate);
        EventHandling.EventUtils.AddListener(EventConstants.OnSendPlayerCorrect, OnSendPlayerCorrect);
    }

    // Start is called before the first frame update
    protected void Start()
    {

        Debug.Log("GameplayManager Start");

        _stateManager = new GameStateManager(_connection);
        _stateManager.OnGameInitialized += OnGameInitializedReceived;
        _stateManager.OnPlayerReady += OnPlayerReadyReceived;
        _stateManager.OnPlayerCorrect += OnPlayerCorrectReceived;
        _stateManager.OnPlayerWin += OnPlayerWinReceived;
        _stateManager.OnGameOver += OnGameOverReceived;


        if (_connection.BattleConnection.Self != null && _connection.BattleConnection.Opponents != null)
        {
            SetInitialPlayerState();
        }

    }

    private void SetInitialPlayerState()
    {
        Debug.Log("SetInitialPlayerState " + _connection.BattleConnection.Opponents.Count());

        _self = _connection.BattleConnection.Self;

        player.GetComponent<Player>().Id = _self.UserId;

        IsHost = (_self.UserId == _connection.BattleConnection.HostId);

        foreach (var presence in _connection.BattleConnection.Opponents)
        {
            if (presence.UserId != _connection.BattleConnection.Self.UserId)
            {
                _enemy = presence;
                enemy.GetComponent<Enemy>().Id = _enemy.UserId;
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

    private void OnGameRoundUpdate(EventHandling.EventArgs eventArgs)
    {
        Debug.Log("OnGameRoundUpdate");
        var updatedRound = Convert.ToInt32(eventArgs.args[0]);
        currentRound = updatedRound;
        Debug.LogFormat("Current Round: {0}", currentRound);
    }

    private void OnSendPlayerCorrect(EventHandling.EventArgs eventArgs)
    {
        Debug.Log("OnSendPlayerCorrect");
        SendOnPlayerCorrect();
    }

    private void SendOnPlayerCorrect()
    {
        Debug.Log("SendOnPlayerCorrect");
        var message = new MatchMessageOnPlayerCorrect(_self.UserId);
        _stateManager.SendMatchStateMessage<MatchMessageOnPlayerCorrect>(MatchMessageType.PlayerCorrect, message);
    }

    private void OnPlayerCorrectReceived(MatchMessageOnPlayerCorrect message)
    {
        Debug.LogFormat("OnPlayerCorrectReceived: {0} | self: {1} | enemy: {2}", message.PlayerId, _self.UserId, _enemy.UserId);
        if (message.PlayerId == _enemy.UserId)
        {
            Debug.Log("OnPlayerCorrectReceived damaged by opponent");
            EventHandling.EventUtils.DispatchEvent(EventConstants.OnEnemyCorrect, (float)(100 / GameConfigurationManager.Instance.GameConfiguration.GameRounds));
        }
    }

    private void SendOnPlayerWin()
    {
        Debug.Log("SendOnPlayerWin");
        var message = new MatchMessageOnPlayerWin(_self.UserId);
        _stateManager.SendMatchStateMessage<MatchMessageOnPlayerWin>(MatchMessageType.PlayerWin, message);
    }

    private void OnPlayerWinReceived(MatchMessageOnPlayerWin message)
    {
        Debug.Log("OnPlayerWinReceived");
        if (message.PlayerId == _enemy.UserId)
        {
            isOver = true;
            Debug.Log("You Lose");
        }
    }

    private void SendOnGameOver()
    {
        var message = new MatchMessageOnGameOver(_enemy.UserId);
        _stateManager.SendMatchStateMessage<MatchMessageOnGameOver>(MatchMessageType.GameOver, message);
    }

    private void OnGameOverReceived(MatchMessageOnGameOver obj)
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isGameEndScreen)
        {
            return;
        }
        else
        {
            //Check if the game is over and winner is concluded
            if (isOver)
            {
                //stop gameplay
                Debug.Log("Game Over");
                return;
            }
            else
            {
                //Check if player is the winner
                if (isWinner)
                {
                    Debug.Log("Player Win");
                    //set game is over to stop gameplay
                    isOver = true;
                    SendOnPlayerWin();
                    return;
                }
                else
                {
                    //winner isn't concluded
                    //check if the game is started
                    if (isPlaying)
                    {
                        //check if player is completed the rounds
                        if (currentRound > maxRounds)
                        {
                            Debug.Log("currentRound > maxRounds");
                            isWinner = true;
                            return;
                        }
                    }
                    else
                    {
                        //game isn't started yet
                        if (playersReadyCount == 2)
                        {
                            //2 players are ready
                            Debug.Log("IsPlaying");
                            isPlaying = true;
                            return;
                        }
                    }
                }
            }
        }
      
    }

    protected override void OnDestroy()
    {
        EventHandling.EventUtils.RemoveListener(EventConstants.OnGameInitialized, OnGameInitialized);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnGameRoundUpdate, OnGameRoundUpdate);
        EventHandling.EventUtils.RemoveListener(EventConstants.OnSendPlayerCorrect, OnSendPlayerCorrect);
        base.OnDestroy();
    }

    private async void OnApplicationQuit()
    {
        await _connection.Socket.CloseAsync();
    }
}
