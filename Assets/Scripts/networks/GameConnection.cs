using Nakama;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConnection",
menuName = GameConstants.CreateAssetMenu_GameConnection)]
public class GameConnection : ScriptableObject
{
    private IClient _client;
    public ISession Session { get; set; }
    public IApiAccount Account { get; set; }
    private ISocket _socket;
    public IClient Client => _client;
    public ISocket Socket => _socket;
    public BattleConnection BattleConnection { get; set; }

    public void Init(IClient client, ISocket socket, IApiAccount account, ISession session)
    {
        _client = client;
        _socket = socket;
        Account = account;
        Session = session;
        BattleConnection = new BattleConnection();
    }

}
