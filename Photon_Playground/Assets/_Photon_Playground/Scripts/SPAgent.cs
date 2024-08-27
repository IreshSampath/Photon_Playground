using TMPro;
using UnityEngine;

public class SPAgent : MonoBehaviour
{
    [SerializeField] TMP_Text _nickname;
    [SerializeField] float _speed;
    [SerializeField] float _horizontalSpeed;
    [SerializeField] float _verticalSpeed;

    public float rotationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _nickname.text = PlayerPrefs.GetString("PlayerName");
    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxis("Horizontal");
        float vValue = Input.GetAxis("Vertical");

        Vector3 movemtDirection = new Vector3(hValue, 0, vValue);
        movemtDirection = Vector3.ClampMagnitude(movemtDirection, 1);

        transform.Translate(movemtDirection * Time.deltaTime * _speed);

        float h = _horizontalSpeed * Input.GetAxis("Mouse X");
        float v = _verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, h, 0);
    }

}
