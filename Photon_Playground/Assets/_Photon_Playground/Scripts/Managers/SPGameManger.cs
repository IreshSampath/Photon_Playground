using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SPGameManger : MonoBehaviour
{
    [SerializeField] GameObject _agent;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
