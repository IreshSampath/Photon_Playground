using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPNetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField] MainMenuManager _mainMenuManager;
    [SerializeField] GameObject _agent;

    IEnumerator _coroutine;
    //TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);

    #region Monobihaviour methods

    // Set up internal variables and make all of cached GetComponent calls.
    void Awake()
    {
        // Currently loaded scene is synchronized across all clients in a room
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #endregion

    #region Public methods

    // Start Multiplayer and connect to the Photon server
    public void StartMultiplayer()
    {
        _mainMenuManager.SelectMultiplayer();

         _coroutine = _mainMenuManager.PrintConsole("<color=yellow>Connecting...</color>");
        StartCoroutine(_coroutine);

        _mainMenuManager.CreateJoinPanel.SetActive(false);

        //Connect to the Master server
        PhotonNetwork.ConnectUsingSettings();
    }

    // Attempts to join lobby
    public void JoinLobby()
    {
        //PhotonNetwork.JoinLobby(_customLobby);
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
        PhotonNetwork.JoinLobby();
    }

    // Attempts to create a room
    public void CreateRoom()
    {
        if (_mainMenuManager.PlayerNameInputFieldMP.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _mainMenuManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if(_mainMenuManager.CreateRoomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _mainMenuManager.PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            RoomOptions options = new RoomOptions();
            options.IsVisible = !_mainMenuManager.IsPrivateToggle.isOn;
            options.MaxPlayers = _mainMenuManager.MaxPlayerInputField.text != "" ? (byte) Int32.Parse(_mainMenuManager.MaxPlayerInputField.text) : 0;

            PhotonNetwork.NickName = _mainMenuManager.PlayerNameInputFieldMP.text;
            PhotonNetwork.CreateRoom(_mainMenuManager.CreateRoomNameInputField.text, options);
        }
    }

    // Attempts to join a public/private room
    public void JoinRoomPrivate()
    {
        if (_mainMenuManager.PlayerNameInputFieldMP.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _mainMenuManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if (_mainMenuManager.JoinRoomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _mainMenuManager.PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.NickName = _mainMenuManager.PlayerNameInputFieldMP.text;
            PhotonNetwork.JoinRoom(_mainMenuManager.JoinRoomNameInputField.text);
        }
    }

    // Attempts to join a public room
    public void JoinRoomPublic(GameObject go)
    {
        if (_mainMenuManager.PlayerNameInputFieldMP.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = _mainMenuManager.PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.NickName = _mainMenuManager.PlayerNameInputFieldMP.text;
            PhotonNetwork.JoinRoom(go.name);
        }
    }

    // This is for Same Scene
    /**public void LeaveRoom()
    //{
    //    _mainMenuManager.CreateJoinPanel.SetActive(true);
    //    _mainMenuManager.GameHUDPanel.SetActive(false);

    //    PhotonNetwork.LeaveRoom();
    }**/

    // Attempts to leave server
    public void LeaveServer()        
    {
        PhotonNetwork.Disconnect();
    }

    // Load the scence allow for master client only
    public void LoadScene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.LoadLevel(_mainMenuManager.SceneNumber);
    }

    #endregion

    #region PunCallbacks methods

    // Call this after connected to the server
    public override void OnConnectedToMaster()
    {
        StopCoroutine(_coroutine);
        _coroutine = _mainMenuManager.PrintConsole("<color=green>Connected to the Master Server</color>");
        StartCoroutine(_coroutine);

        _mainMenuManager.GameSelectPanel.SetActive(false);
        _mainMenuManager.CreateJoinPanel.SetActive(true);

        //After we connected to Master server, join the Lobby
        JoinLobby();
    }

    // Call this when room list is updated
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _mainMenuManager.CreateRoomButtonList(roomList);
    }

    // Call this after created a room 
    public override void OnCreatedRoom()
    {
        StopCoroutine(_coroutine);
        _coroutine = _mainMenuManager.PrintConsole("<color=green>Created room is: </color>" + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(_coroutine);
    }

    // Call this after joined to a room
    public override void OnJoinedRoom()
    {
        // Use this for the same Scene
        /* 
        StopCoroutine(_coroutine);
        _coroutine = _mainMenuManager.PrintConsole("<color=green>Joined room is: </color>" + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(_coroutine);

        _mainMenuManager.CreateJoinPanel.SetActive(false);
        _mainMenuManager.GameHUDPanel.SetActive(true);

        PhotonNetwork.Instantiate(_agent.name, new Vector3(0, 3, 0), Quaternion.identity);
        */

        // Use this for load another scene
        LoadScene();
    }

    // Call this when disconnecedt the sever
    public override void OnDisconnected(DisconnectCause cause)
    {
        StopCoroutine(_coroutine);
        _coroutine = _mainMenuManager.PrintConsole("<color=red>" + cause + "</color>");
        StartCoroutine(_coroutine);

        _mainMenuManager.GameSelectPanel.SetActive(true);
    }

    // Call this when failed the room creation
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StopCoroutine(_coroutine);
        _coroutine = _mainMenuManager.PrintConsole("<color=red>" + message + "</color>");
        StartCoroutine(_coroutine);
    }

    // Call this when failed the room joining
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StopCoroutine(_coroutine);
        _coroutine = _mainMenuManager.PrintConsole("<color=red>" + message + "</color>");
        StartCoroutine(_coroutine);
    }

    #endregion
}
