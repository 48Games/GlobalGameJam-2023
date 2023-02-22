using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    [SerializeField]
    private GameObject loadUI;
    [SerializeField]
    private GameObject lobbyUI;
    [SerializeField]
    private TextMeshProUGUI roomName;

    #endregion

    #region Private Fields

    /// &lt;summary&gt;
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// &lt;/summary&gt;
    string gameVersion = "1";

    #endregion

    #region MonoBehaviour CallBacks

    /// &lt;summary&gt;
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// &lt;/summary&gt;
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// &lt;summary&gt;
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// &lt;/summary&gt;
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnJoinedRoom()
    {
        GameObject.FindGameObjectWithTag("Transition").GetComponent<Transition>().DoTransition(
            () =>
            {
                loadUI.SetActive(false);
                lobbyUI.SetActive(true);
                PhotonNetwork.Instantiate("PlayerInLobbyOnline", Vector3.zero, Quaternion.Euler(0, 180, 0)).GetComponent<PlayerInLobbyOnline>();
                roomName.text = LobbyData.RoomName;
            }
        );
    }

    public override void OnLeftRoom()
    {
        GameObject.FindGameObjectWithTag("Transition").GetComponent<Transition>().DoTransition(
            () =>
            {
                loadUI.SetActive(true);
                lobbyUI.SetActive(false);
            }
        );
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room doesn't exist " + returnCode +"/"+message);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player joined");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Player left");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    #endregion

    #region Public Methods

    /// &lt;summary&gt;
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// &lt;/summary&gt;
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            if (LobbyData.CreateGame)
            {
                // We are the host
                LobbyData.RoomName = GetRandomString(6);
                RoomOptions options = new RoomOptions();
                options.IsVisible = true;
                options.IsOpen = true;
                //PhotonNetwork.CreateRoom(LobbyData.RoomName, options);
                PhotonNetwork.CreateRoom(LobbyData.RoomName, options);
            }
            else
            {
                // We join the room
                //PhotonNetwork.JoinRandomRoom();
                PhotonNetwork.JoinRoom(LobbyData.RoomName);
            }
        }
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public void Play()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MultiplayerScene");
        }
    }

    #endregion

    #region Private methods

    private string GetRandomString(int length)
    {
        string myString = "";
        const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int i = 0; i < length; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }
        return myString;
    }

    #endregion

}


