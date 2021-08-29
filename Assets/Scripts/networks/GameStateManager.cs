using Nakama;
using Nakama.TinyJson;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager
{
    public event Action<MatchMessageOnPlayerCorrect> OnPlayerCorrect;
    public event Action<MatchMessageOnPlayerDead> OnPlayerDead;
    public event Action<MatchMessageOnPlayerReady> OnPlayerReady;
    public event Action<MatchMessageOnGameInitialized> OnGameInitialized;
    public event Action<MatchMessageOnGameOver> OnGameOver;

    private bool _isLeaving;
    private GameConnection _connection;

    public GameStateManager(GameConnection connection)
    {
        _connection = connection;

        _connection.Socket.ReceivedMatchPresence += OnMatchPresence;
        _connection.Socket.ReceivedMatchState += ReceiveMatchStateMessage;
    }

    public async void LeaveGame()
    {
        if (_isLeaving)
        {
            return;
        }

        _isLeaving = true;

        _connection.Socket.ReceivedMatchPresence -= OnMatchPresence;
        _connection.Socket.ReceivedMatchState -= ReceiveMatchStateMessage;

        try
        {
            await _connection.Socket.LeaveMatchAsync(_connection.BattleConnection.MatchId);
        }catch (Exception e)
        {
            Debug.LogWarning("Error leaving match: " + e.Message);
        }

        _connection.BattleConnection = null;

        SceneManager.LoadSceneAsync(GameConfigurationManager.Instance.GameConfiguration.SceneNameMainMenu);
        SceneManager.UnloadSceneAsync(GameConfigurationManager.Instance.GameConfiguration.SceneNameGameplay);
    }

    public void SendMatchStateMessage<T>(MatchMessageType opCode, T message)
              where T : MatchMessage<T>
    {
        try
        {
            string json = MatchMessage<T>.ToJson(message);

            _connection.Socket.SendMatchStateAsync(_connection.BattleConnection.MatchId, (long)opCode, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while sending match state: " + e.Message);
        }
    }

    public void SendMatchStateMessageSelf<T>(MatchMessageType opCode, T message)
            where T : MatchMessage<T>
    {
        //Choosing which event should be invoked basing on opCode and firing event
        switch (opCode)
        {
            case MatchMessageType.GameInitialized:
                OnGameInitialized?.Invoke(message as MatchMessageOnGameInitialized);
                break;
            case MatchMessageType.GameOver:
                OnGameOver?.Invoke(message as MatchMessageOnGameOver);
                break;
            case MatchMessageType.PlayerReady:
                OnPlayerReady?.Invoke(message as MatchMessageOnPlayerReady);
                break;
            case MatchMessageType.PlayerCorrect:
                OnPlayerCorrect?.Invoke(message as MatchMessageOnPlayerCorrect);
                break;
            case MatchMessageType.PlayerDead:
                OnPlayerDead?.Invoke(message as MatchMessageOnPlayerDead);
                break;
            default:
                break;
        }
    }

    public void ReceiveMatchStateHandle(long opCode, string messageJson)
    {
        //Choosing which event should be invoked basing on opCode, then parsing json to MatchMessage class and firing event
        switch ((MatchMessageType)opCode)
        {
            case MatchMessageType.GameInitialized:
                MatchMessageOnGameInitialized matchMessageOnGameInitialized = MatchMessageOnGameInitialized.Parse(messageJson);
                OnGameInitialized?.Invoke(matchMessageOnGameInitialized);
                break;
            case MatchMessageType.GameOver:
                MatchMessageOnGameOver matchMessageOnGameOver = MatchMessageOnGameOver.Parse(messageJson);
                OnGameOver?.Invoke(matchMessageOnGameOver);
                break;
            case MatchMessageType.PlayerReady:
                MatchMessageOnPlayerReady matchMessageOnPlayerReady = MatchMessageOnPlayerReady.Parse(messageJson);
                OnPlayerReady?.Invoke(matchMessageOnPlayerReady);
                break;
            case MatchMessageType.PlayerCorrect:
                MatchMessageOnPlayerCorrect matchMessageOnPlayerCorrect = MatchMessageOnPlayerCorrect.Parse(messageJson);
                OnPlayerCorrect?.Invoke(matchMessageOnPlayerCorrect);
                break;
            case MatchMessageType.PlayerDead:
                MatchMessageOnPlayerDead matchMessageOnPlayerDead = MatchMessageOnPlayerDead.Parse(messageJson);
                OnPlayerDead?.Invoke(matchMessageOnPlayerDead);
                break;
            default:
                break;
        }
    }

    private void OnMatchPresence(IMatchPresenceEvent e)
    {
        if (e.Leaves.Count() > 0)
        {
            Debug.LogWarning($"OnMatchPresence() User(s) left the game");
            LeaveGame();
        }
    }

    private void ReceiveMatchStateMessage(IMatchState matchState)
    {
        string messageJson = System.Text.Encoding.UTF8.GetString(matchState.State);

        if (string.IsNullOrEmpty(messageJson))
        {
            return;
        }

        ReceiveMatchStateHandle(matchState.OpCode, messageJson);
    }

}
