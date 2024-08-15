using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    GameObject _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = _camera.transform.forward;
    }
}
