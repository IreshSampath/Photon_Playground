using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Instance
    // public static NetworkManager instance;
    [SerializeField] GameObject _agent;

    [SerializeField] TMP_Text _colsolePrint;

    [SerializeField] TMP_InputField _roomNameInputField;
    [SerializeField] TMP_InputField _playerNameInputField;
    [SerializeField] GameObject _createJoinPanel;

    private IEnumerator _coroutine;

    private void Awake()
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

    // Start is called before the first frame update
    //void Start()
    //{



    //}

    IEnumerator PrintConsole(string printText)
    {
        _colsolePrint.text = printText;
        yield return new WaitForSeconds(5);
        _colsolePrint.text = "";
    }

    #region public methods

    public void StartMultiplayer()
    {
        _coroutine = PrintConsole("<color=yellow>Connecting...</color>");
        StartCoroutine(_coroutine);
        _createJoinPanel.SetActive(false);

        //Connect to the Master server
        PhotonNetwork.ConnectUsingSettings();
    }

    // Attempts to create a room
    public void CreateRoom()
    {
        if (_roomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if (_playerNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.CreateRoom(_roomNameInputField.text);
        }
    }

    // Attempts to join a room
    public void JoinRoom()
    {
        if(_roomNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = PrintConsole("<color=red>Room name is required</color>");
            StartCoroutine(_coroutine);
        }
        else if (_playerNameInputField.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PhotonNetwork.JoinRoom(_roomNameInputField.text);
        }
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
        _coroutine = PrintConsole("<color=green>Connected to the Master Server</color>");
        StartCoroutine(_coroutine);

        _createJoinPanel.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        StopCoroutine(_coroutine);
        _coroutine = PrintConsole("<color=green>Created room is: </color>" + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(_coroutine);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = _playerNameInputField.text;

        StopCoroutine(_coroutine);
        _coroutine = PrintConsole("<color=green>Joined room is: </color>" + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(_coroutine);

        _createJoinPanel.SetActive(false);
        PhotonNetwork.Instantiate(_agent.name, new Vector3(0, 3, 0), Quaternion.identity);

        //LoadScene(1);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            print(room.Name);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StopCoroutine(_coroutine);
        _coroutine = PrintConsole("<color=red>" + message + "</color>");
        StartCoroutine(_coroutine);

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StopCoroutine(_coroutine);
        _coroutine = PrintConsole("<color=red>" + message + "</color>");
        StartCoroutine(_coroutine);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        StopCoroutine(_coroutine);
        _coroutine = PrintConsole("<color=red>" + cause + "</color>");
        StartCoroutine(_coroutine);
    }

    #endregion
}
