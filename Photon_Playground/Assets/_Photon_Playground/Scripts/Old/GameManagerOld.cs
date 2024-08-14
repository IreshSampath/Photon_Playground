using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

namespace com.GameArtGames.PhotonPlayground
{
    public class GameManagerOld : MonoBehaviourPunCallbacks
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #region MonoBehaviour Callbacks

        void Awake()
        {
            //// #Important
            //// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            //if (photonView.IsMine)
            //{
            //    PlayerManager.LocalPlayerInstance = this.gameObject;
            //}
            //// #Critical
            //// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            //DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            //// #Important
            //// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            //if (photonView.IsMine)
            //{
            //    PlayerManager.LocalPlayerInstance = this.gameObject;
            //}
            //// #Critical
            //// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            //DontDestroyOnLoad(this.gameObject);

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }
        #endregion

        #region Photon Callbacks

        public override void OnJoinedRoom()
        {
            // Note: it is possible that this monobehaviour is not created (or active) when OnJoinedRoom happens
            // due to that the Start() method also checks if the local player character was network instantiated!
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        /// Called when the local player left the room. We need to load the launcher scene.
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
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

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        public void QuitApplication()
        {
            Application.Quit();
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Photon_Playground " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion
    }
}
