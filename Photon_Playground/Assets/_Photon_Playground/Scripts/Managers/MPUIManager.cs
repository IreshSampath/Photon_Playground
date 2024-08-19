using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MPUIManager : MonoBehaviour
{
    public GameObject GameSelectPanel;
    public GameObject CreateJoinPanel;
    public GameObject GameHUDPanel;

    public TMP_InputField PlayerNameInputField;
    public TMP_InputField CreateRoomNameInputField;
    public TMP_InputField JoinRoomNameInputField;
    public TMP_InputField MaxPlayerInputField;
    public Toggle IsPrivateToggle;

    [SerializeField] TMP_Text _colsolePrintTxt;
    [SerializeField] Transform _roomListContainer;
    [SerializeField] GameObject _roomButtonPrefab;
    [SerializeField] GameObject _noRoomsTxt;

    List<GameObject> _roomBtns = new List<GameObject>();

    public void LoadSingleplayer()
    {
        SceneManager.LoadScene(1);
    }

    public IEnumerator PrintConsole(string printText)
    {
        _colsolePrintTxt.text = printText;
        yield return new WaitForSeconds(5);
        _colsolePrintTxt.text = "";
    }

    public void CreateRoomButton(List<RoomInfo> rooms)
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
