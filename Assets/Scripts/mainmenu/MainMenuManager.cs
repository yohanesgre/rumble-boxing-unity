using Nakama;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private GameConnection _connection;
    [SerializeField] private List<IUserPresence> connectedPlayers = new List<IUserPresence>(2);

    [Header("Menu Containers")]
    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuQuickMatch;
    [SerializeField] private GameObject menuPrivateMatch;

    [Header("Menu Main Container")]
    [SerializeField] private Button buttonPrivateMatch;
    [SerializeField] private Button buttonQuickMatch;

    [Header("Menu Quick Match Container")]
    [SerializeField] private Button buttonCancelFindMatch;

    [Header("Menu Private Match Container")]
    [SerializeField] private GameObject panelTop;
    [SerializeField] private Text textMatchCode;
    [SerializeField] private Button buttonCreateMatch;
    [SerializeField] private Button buttonClosePrivateMatch;
    [Space(15)]
    [SerializeField] private GameObject panelBottom;
    [SerializeField] private InputField inputMatchCode;
    [SerializeField] private Button buttonJoinMatch;


    private IMatchmakerTicket _ticket;
    private IUserPresence _self;
    private IMatch _privateMatch;


    private void Awake()
    {
        buttonPrivateMatch.onClick.AddListener(() =>
        {
            HideMainMenu();
            ShowMenuPrivateMatch();
        });

        buttonQuickMatch.onClick.AddListener(() =>
        {
            HideMainMenu();
            ShowMenuQuickMatch();
            StartQuickMatch();
        });

        buttonClosePrivateMatch.onClick.AddListener(() =>
        {
            HideMenuPrivateMatch();
            ShowMenuMain();
        });

        buttonCancelFindMatch.onClick.AddListener(() =>
        {
            HideMenuQuickMatch();
            StopQuickMatch();
            ShowMenuMain();
        });

        buttonCreateMatch.onClick.AddListener(() =>
        {
            if (_privateMatch == null)
            {
                CreatePrivateMatch();
                buttonCreateMatch.gameObject.GetComponentInChildren<Text>().text = "Leave";
            }
            else
            {
                LeavePrivateMatch();
                buttonCreateMatch.gameObject.GetComponentInChildren<Text>().text = "Create";
            }

        });

        buttonJoinMatch.onClick.AddListener(() =>
        {
            JoinPrivateMatch();
        });
    }

    // Start is called before the first frame update
    private async void Start()
    {
        if (_connection != null)
        {
            if (_connection.Session == null)
            {
                string deviceId = GetDeviceId();
                if (!string.IsNullOrEmpty(deviceId))
                {
                    PlayerPrefs.SetString(GameConstants.DeviceIdKey, deviceId);
                }

                await InitializeGame(deviceId);
            }
        }
    }

    private async Task InitializeGame(string deviceId)
    {
        var client = new Client("http", "localhost", 7350, "defaultkey", UnityWebRequestAdapter.Instance);
        client.Timeout = 5;

        var socket = client.NewSocket(useMainThread: true);

        string authToken = PlayerPrefs.GetString(GameConstants.AuthTokenKey, null);
        bool isAuthToken = !string.IsNullOrEmpty(authToken);

        string refreshToken = PlayerPrefs.GetString(GameConstants.RefreshTokenKey, null);

        ISession session = null;

        // refresh token can be null/empty for initial migration of client to using refresh tokens.
        if (isAuthToken)
        {
            session = Session.Restore(authToken, refreshToken);

            // Check whether a session is close to expiry.
            if (session.HasExpired(DateTime.UtcNow.AddDays(1)))
            {
                try
                {
                    // get a new access token
                    session = await client.SessionRefreshAsync(session);
                }
                catch (ApiResponseException)
                {
                    // get a new refresh token
                    session = await client.AuthenticateDeviceAsync(deviceId);
                    PlayerPrefs.SetString(GameConstants.RefreshTokenKey, session.RefreshToken);
                }

                PlayerPrefs.SetString(GameConstants.AuthTokenKey, session.AuthToken);
            }
        }
        else
        {
            session = await client.AuthenticateDeviceAsync(deviceId);
            PlayerPrefs.SetString(GameConstants.AuthTokenKey, session.AuthToken);
            PlayerPrefs.SetString(GameConstants.RefreshTokenKey, session.RefreshToken);
        }

        try
        {
            await socket.ConnectAsync(session);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error connecting socket: " + e.Message);
        }

        IApiAccount account = null;

        try
        {
            account = await client.GetAccountAsync(session);
        }
        catch (ApiResponseException e)
        {
            Debug.LogError("Error getting user account: " + e.Message);
        }

        _connection.Init(client, socket, account, session);
    }

    private string GetDeviceId()
    {
        string deviceId = "";

        deviceId = PlayerPrefs.GetString(GameConstants.DeviceIdKey);

        if (string.IsNullOrWhiteSpace(deviceId))
        {
            // Ordinarily, we would use SystemInfo.deviceUniqueIdentifier but for the purposes
            // of this demo we use Guid.NewGuid() so that developers can test against themselves locally.
            // Also note: SystemInfo.deviceUniqueIdentifier is not supported in WebGL.
            deviceId = Guid.NewGuid().ToString();
        }
        return deviceId;
    }

    private async void OnApplicationQuit()
    {
        if (_connection != null && _connection.Socket != null)
        {
            await _connection.Socket.CloseAsync();
        }
    }

    public async void StartQuickMatch()
    {
        _connection.Socket.ReceivedMatchmakerMatched += OnMatchmakerMatched;

        // Join the matchmaker
        try
        {
            // Acquires matchmaking ticket used to join a match
            Debug.Log("start matchmaking");
            _ticket = await _connection.Socket.AddMatchmakerAsync(
                query: "*",
                minCount: 2,
                maxCount: 2,
                stringProperties: null,
                numericProperties: null);

        }
        catch (Exception e)
        {
            Debug.LogWarning("An error has occured while joining the matchmaker: " + e);
        }
    }

    private void OnMatchmakerMatched(IMatchmakerMatched matched)
    {
        Debug.Log("matchmaker matched called");
        _self = matched.Self.Presence;
        connectedPlayers.Add(_self);
        _connection.BattleConnection.Self = _self;
        _connection.BattleConnection.MatchId = matched.MatchId;
        _connection.Socket.ReceivedMatchmakerMatched -= OnMatchmakerMatched;

        JoinMatchedMatch();
    }

    private async void JoinMatchedMatch(IMatchmakerMatched matched = null)
    {
        _connection.Socket.ReceivedMatchPresence += WaitPlayerToJoinMatchedMatch;
        if (matched != null)
        {
            IMatch match = await _connection.Socket.JoinMatchAsync(matched);
        }
    }

    private void WaitPlayerToJoinMatchedMatch(IMatchPresenceEvent presenceEvent)
    {
        Debug.Log("WaitPlayerToJoinMatchedMatch");
        if (presenceEvent.Joins.Count() > 0)
        {
            foreach (var presence in presenceEvent.Joins)
            {
                connectedPlayers.Add(presence);
            }
        }
        else if (presenceEvent.Leaves.Count() > 0)
        {
            foreach (var presence in presenceEvent.Leaves)
            {
                connectedPlayers.Remove(presence);
            }
        }
        if (connectedPlayers.Count() >= 2)
        {
            _connection.BattleConnection.HostId = connectedPlayers.First().UserId;
            connectedPlayers.Remove(_self);
            _connection.BattleConnection.Opponents = connectedPlayers;
            _connection.Socket.ReceivedMatchPresence -= WaitPlayerToJoinMatchedMatch;
            Debug.Log("Load Gameplay");
            SceneManager.LoadScene(GameConfigurationManager.Instance.GameConfiguration.SceneNameGameplay);
        }
    }

    public async void StopQuickMatch(bool isMuteSoundManager = false)
    {
        try
        {
            ResetBattleConnection();
            await _connection.Socket.RemoveMatchmakerAsync(_ticket);
        }
        catch (Exception e)
        {
            Debug.LogWarning("An error has occured while removing from matchmaker: " + e);
        }

        _connection.Socket.ReceivedMatchmakerMatched -= OnMatchmakerMatched;
        _ticket = null;
    }

    private async void CreatePrivateMatch()
    {
        _privateMatch = await _connection.Socket.CreateMatchAsync();
        textMatchCode.text = _privateMatch.Id;
        JoinPrivateMatch(_privateMatch.Id);
    }
    private async void JoinPrivateMatch(String _matchId = "")
    {
        var matchId = !String.IsNullOrEmpty(_matchId) ? _matchId: inputMatchCode.text;
        var match = await _connection.Socket.JoinMatchAsync(matchId);
        foreach (var presence in match.Presences)
        {
            _self = presence;
            connectedPlayers.Add(_self);
            _connection.BattleConnection.Self = _self;
            _connection.BattleConnection.HostId = _self.UserId;
            _connection.BattleConnection.MatchId = match.Id;
            Debug.LogFormat("JoinPrivateMatch {0} | {1}", connectedPlayers.Count(), _self.UserId);
        }

        _connection.Socket.ReceivedMatchPresence += WaitPlayerToJoinPrivateMatch;
    }

    private async void LeavePrivateMatch()
    {
        _connection.Socket.ReceivedMatchPresence -= WaitPlayerToJoinPrivateMatch;
        await _connection.Socket.LeaveMatchAsync(_privateMatch);
        ResetBattleConnection();
        textMatchCode.text = "";
        _privateMatch = null;
    }

    private void WaitPlayerToJoinPrivateMatch(IMatchPresenceEvent presenceEvent)
    {
        if (presenceEvent.Joins.Count() > 0)
        {
            foreach (var presence in presenceEvent.Joins)
            {
                connectedPlayers.Add(presence);
            }
        }
        else if (presenceEvent.Leaves.Count() > 0)
        {
            foreach (var presence in presenceEvent.Leaves)
            {
                connectedPlayers.Remove(presence);
            }
        }

        if (connectedPlayers.Count() >= 2)
        {
            connectedPlayers.Remove(_self);
            _connection.BattleConnection.Opponents = connectedPlayers;
            _connection.Socket.ReceivedMatchPresence -= WaitPlayerToJoinPrivateMatch;
            SceneManager.LoadScene(GameConfigurationManager.Instance.GameConfiguration.SceneNameGameplay);
        }
    }

    private void ResetBattleConnection()
    {
        connectedPlayers.Clear();
        _connection.BattleConnection.Self = null;
        _connection.BattleConnection.Opponents = null;
        _connection.BattleConnection.HostId = "";
        _connection.BattleConnection.MatchId = "";
    }

    private void ShowMenuMain()
    {
        menuMain.SetActive(true);
    }

    private void HideMainMenu()
    {
        menuMain.SetActive(false);
    }

    private void ShowMenuQuickMatch()
    {
        menuQuickMatch.SetActive(true);
    }

    private void HideMenuQuickMatch()
    {
        menuQuickMatch.SetActive(false);
    }

    private void ShowMenuPrivateMatch()
    {
        menuPrivateMatch.SetActive(true);
    }

    private void HideMenuPrivateMatch()
    {
        menuPrivateMatch.SetActive(false);
    }

    public void CopyMatchCode()
    {
        ClipboardExtension.CopyToClipboard(textMatchCode.text);
        Debug.Log("Copy Match Code");
#if UNITY_ANDROID
        Debug.Log("Android Copy Match Code");
        ShowAndroidToast();
#else
        Debug.Log("No Toast setup for this platform.");
#endif
    }

    private void ShowAndroidToast(string toastText)
    {
        //create a Toast class object
        AndroidJavaClass toastClass =
                    new AndroidJavaClass("android.widget.Toast");

        //create an array and add params to be passed
        object[] toastParams = new object[3];
        AndroidJavaClass unityActivity =
          new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        toastParams[0] =
                     unityActivity.GetStatic<AndroidJavaObject>
                               ("currentActivity");
        toastParams[1] = toastText;
        toastParams[2] = toastClass.GetStatic<int>
                               ("LENGTH_LONG");

        //call static function of Toast class, makeText
        AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>
                                      ("makeText", toastParams);

        //show toast
        toastObject.Call("show");
    }
}
