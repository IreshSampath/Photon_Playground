using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField] UIManager _uIManager;
    [SerializeField] GameObject _agent;

    // Instance
    // public static NetworkManager instance;

    IEnumerator _coroutine;

    TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);
    List<RoomInfo> rooms = new List<RoomInfo>();

    void Awake()
    {
        // If an instance alredy exits and it's not this one - destroy us
        //if (instance != null && instance != this)
        //{
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    // Set the instance
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region public methods

    public void StartMultiplayer()
    {
        _coroutine = _uIManager.PrintConsole("<color=yellow>Connecting...</color>");
        StartCoroutine(_coroutine);

        _uIManager.CreateJoinPanel.SetActive(false);

        //Connect to the Master server
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinLobby()
    {
        //PhotonNetwork.JoinLobby(customLobby);
        PhotonNetwork.JoinLobby();
    }

    // Attempts to create a room
    public void CreateRoom()
    {
        if (_uIManager.RoomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if (_uIManager.PlayerNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.CreateRoom(_uIManager.RoomNameInputField.text);
        }
    }

    // Attempts to join a room
    public void JoinRoom(GameObject go)
    {
        if (_uIManager.PlayerNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.JoinRoom(go.name);
        }
    }

    public void LeaveServer()
    {
        PhotonNetwork.Disconnect();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        _uIManager.CreateJoinPanel.SetActive(true);
        _uIManager.GameHUDPanel.SetActive(false);
    }

    // Change the scence using Photon's system
    public void LoadScene(int sceneName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    #endregion

    #region punCallbacks methods
    
    public override void OnConnectedToMaster()
    {
        StopCoroutine(_coroutine);
        _coroutine = _uIManager.PrintConsole("<color=green>Connected to the Master Server</color>");
        StartCoroutine(_coroutine);

        _uIManager.GameSelectPanel.SetActive(false);
        _uIManager.CreateJoinPanel.SetActive(true);

        //After we connected to Master server, join the Lobby
        JoinLobby();
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _uIManager.CreateRoomButton(roomList);
    }

    public override void OnCreatedRoom()
    {
        StopCoroutine(_coroutine);
        _coroutine = _uIManager.PrintConsole("<color=green>Created room is: </color>" + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(_coroutine);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = _uIManager.PlayerNameInputField.text;

        StopCoroutine(_coroutine);
        _coroutine = _uIManager.PrintConsole("<color=green>Joined room is: </color>" + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(_coroutine);

        _uIManager.CreateJoinPanel.SetActive(false);
        _uIManager.GameHUDPanel.SetActive(true);

        PhotonNetwork.Instantiate(_agent.name, new Vector3(0, 3, 0), Quaternion.identity);

        //LoadScene(1);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        StopCoroutine(_coroutine);
        _coroutine = _uIManager.PrintConsole("<color=red>" + cause + "</color>");
        StartCoroutine(_coroutine);

        _uIManager.GameSelectPanel.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StopCoroutine(_coroutine);
        _coroutine = _uIManager.PrintConsole("<color=red>" + message + "</color>");
        StartCoroutine(_coroutine);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StopCoroutine(_coroutine);
        _coroutine = _uIManager.PrintConsole("<color=red>" + message + "</color>");
        StartCoroutine(_coroutine);
    }

    #endregion
}
