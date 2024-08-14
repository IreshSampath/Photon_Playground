
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    //[SerializeField] PhotonView _agent;
    [SerializeField] GameObject _agent;

    // Start is called before the first frame update
    void Start()
    {
        //if (PhotonNetwork.InRoom && Agent.LocalPlayerInstance == null)
        //{
        //    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

        //    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //    PhotonNetwork.Instantiate(_agent.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        //}
        //else
        //{
        //    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnJoinedRoom()
    {
        //Note: it is possible that this monobehaviour is not created(or active) when OnJoinedRoom happens
        // due to that the Start() method also checks if the local player character was network instantiated!
        //if (Agent.LocalPlayerInstance == null)
        //{
        //    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

        //    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //    PhotonNetwork.Instantiate(_agent.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        //}
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(1);
    }
}
