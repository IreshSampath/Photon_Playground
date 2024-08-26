using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject GameSelectPanel;
    public GameObject CreateJoinPanel;
    public GameObject GameHUDPanel;

    public TMP_InputField PlayerNameInputFieldSP;
    public TMP_Dropdown ScenesDropdownSP;

    public TMP_InputField PlayerNameInputFieldMP;
    public TMP_Dropdown ScenesDropdownMP;

    public TMP_InputField CreateRoomNameInputField;
    public TMP_InputField MaxPlayerInputField;
    public Toggle IsPrivateToggle;

    public TMP_InputField JoinRoomNameInputField;

    public int SceneNumber;

    [SerializeField] TMP_Text _colsolePrintTxt;
    [SerializeField] Transform _roomListContainer;
    [SerializeField] GameObject _roomButtonPrefab;
    [SerializeField] GameObject _noRoomsTxt;

    List<GameObject> _roomBtns = new List<GameObject>();

    IEnumerator _coroutine;
    bool isFullscreen;

    // Set up internal variables and make all of cached GetComponent calls.
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneNumber = 1;
        Cursor.lockState = CursorLockMode.None;
        isFullscreen = false;
    }

    // Attempts to change fullscreen
    public void ChangeFullScreen()
    {
        if (!isFullscreen)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
        isFullscreen = !isFullscreen;
    }

    // Attempts to select single player
    public void SelectSingleplayer()
    {
        PlayerPrefs.SetInt("IsSnglePlayer", 1);
        _coroutine = PrintConsole("");
        StartCoroutine(_coroutine);
    }

    // Attempts to select multiplayer
    public void SelectMultiplayer()
    {
        PlayerPrefs.SetInt("IsSnglePlayer", 0);
    }

    // Attempts to load single player scene
    public void StartSingleplayer()
    {
        if (PlayerNameInputFieldSP.text == "")
        {
            StopCoroutine(_coroutine);
            _coroutine = PrintConsole("<color=red>Player name is required</color>");
            StartCoroutine(_coroutine);
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", PlayerNameInputFieldSP.text);
            SceneManager.LoadScene(SceneNumber);
        }
    }

    // Attempts to print on the console
    public IEnumerator PrintConsole(string printText)
    {
        _colsolePrintTxt.text = printText;
        yield return new WaitForSeconds(5);
        _colsolePrintTxt.text = "";
    }

    // Attempts to create game scene
    public void SelectScene()
    {
        if(PlayerPrefs.GetInt("IsSnglePlayer") == 1)
        {
            SceneNumber = ScenesDropdownSP.value + 1;
        }
        else
        {
            SceneNumber = ScenesDropdownMP.value + 1;
        }
    }

    // Attempts to create room buttons list
    public void CreateRoomButtonList(List<RoomInfo> rooms)
    {
        foreach (RoomInfo room in rooms)
        {
            bool isNew = true;

            if (room.RemovedFromList)
            {
                for (int i = 0; i < _roomBtns.Count(); i++)
                {
                    if (room.Name == _roomBtns[i].transform.GetChild(0).name)
                    {
                        Destroy(_roomBtns[i]);
                        _roomBtns.Remove(_roomBtns[i]);
                    }

                    if(_roomBtns.Count == 0)
                    {
                        _noRoomsTxt.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (GameObject btn in _roomBtns)
                {
                    if (room.Name == btn.transform.GetChild(0).name)
                    {
                        btn.transform.GetChild(0).GetComponent<TMP_Text>().text = room.Name + " " + room.PlayerCount + "/" + room.MaxPlayers;
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    GameObject go = Instantiate(_roomButtonPrefab, _roomListContainer);
                    go.SetActive(true);
                    go.transform.GetChild(0).GetComponent<TMP_Text>().text = room.Name + " " + room.PlayerCount + "/" + room.MaxPlayers;
                    go.transform.GetChild(0).name = room.Name;

                    _roomBtns.Add(go);
                    _noRoomsTxt.SetActive(false);
                }
            }
        }
    }
}
