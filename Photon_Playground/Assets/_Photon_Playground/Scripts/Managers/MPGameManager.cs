
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;

public class MPGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _agent;

    #region MonoBehaviour methods

    // Set up internal variables and make all of cached GetComponent calls.
    void Awake()
    {
       Cursor.lockState = CursorLockMode.Confined;

       PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // When creating the game, checks if the local player character was network instantiated!
        if (PhotonNetwork.InRoom && Agent.LocalPlayerInstance == null)
        {
            //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            // in the room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(_agent.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    #endregion

    #region Public methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private methods

    // Load the scence 
    void LoadScene(int sceneNo)
    {
        PhotonNetwork.LoadLevel(sceneNo);
    }

    #endregion

    #region PunCallbacks

    // Server will disconnect and load scene
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        LoadScene(0);
    }

    // When joined the game, local player character instantiate!
    public override void OnJoinedRoom()
    {
        // When joining the game, checks if the local player character was network instantiated!
        if (PhotonNetwork.InRoom && Agent.LocalPlayerInstance == null)
        {
            // Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            // In the room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(_agent.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
    }

    /*public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadScene(0);
        }
    }*/

    #endregion
}
