using Photon.Pun;
using TMPro;
using UnityEngine;

public class Agent : MonoBehaviourPunCallbacks
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [SerializeField] TMP_Text _nickname;
    [SerializeField] float _speed;
    [SerializeField] float _horizontalSpeed;
    [SerializeField] float _verticalSpeed;

    private void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _nickname.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float hValue = Input.GetAxis("Horizontal");
            float vValue = Input.GetAxis("Vertical");

        Vector3 movemtDirection = new Vector3(hValue, 0, vValue);
        movemtDirection = Vector3.ClampMagnitude(movemtDirection,1);

        transform.Translate(movemtDirection * Time.deltaTime *_speed);

            float h = _horizontalSpeed * Input.GetAxis("Mouse X");
            float v = _verticalSpeed * Input.GetAxis("Mouse Y");
            transform.Rotate(v, h, 0);
        }
    }
}
