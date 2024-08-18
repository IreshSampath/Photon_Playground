using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MPNetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField] MPUIManager _uIManager;
    [SerializeField] GameObject _agent;

    // Instance
    // public static NetworkManager instance;

    IEnumerator _coroutine;

   //TypedLobby customLobby = new TypedLobby("customLobby", LobbyType.Default);

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
        if (_uIManager.PlayerNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if(_uIManager.CreateRoomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            RoomOptions options = new RoomOptions();

            options.IsVisible = !_uIManager.IsPrivateToggle.isOn;

            options.MaxPlayers = _uIManager.MaxPlayerInputField.text != "" ? (byte) Int32.Parse(_uIManager.MaxPlayerInputField.text) : 0;

            PhotonNetwork.CreateRoom(_uIManager.CreateRoomNameInputField.text, options);
        }
    }

    // Attempts to join a room
    public void JoinRoomPrivate()
    {
        if (_uIManager.PlayerNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if (_uIManager.JoinRoomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _uIManager.PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.JoinRoom(_uIManager.JoinRoomNameInputField.text);
        }
    }

    public void JoinRoomPublic(GameObject go)
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

    public void LeaveRoom()
    {
        _uIManager.CreateJoinPanel.SetActive(true);
        _uIManager.GameHUDPanel.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }

    public void LeaveServer()        
    {
        PhotonNetwork.Disconnect();
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

        //LoadScene(2);
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
