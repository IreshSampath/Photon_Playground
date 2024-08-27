using UnityEngine;

public class SPGameManager : MonoBehaviour
{
    [SerializeField] GameObject _agent;

    // Start is called before the first frame update
    void Start()
    {
        // in the room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        GameObject go = Instantiate(_agent, new Vector3(0f, 5f, 0f), Quaternion.identity);
        go.name = PlayerPrefs.GetString("PlayerName");
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}


}
