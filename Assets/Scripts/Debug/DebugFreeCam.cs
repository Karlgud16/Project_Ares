//Handles the FreeCam Movement (Debug)

using UnityEngine;

public class DebugFreeCam : MonoBehaviour
{

    private Vector3 _defaultCameraPos;

    private Quaternion _defaultCameraRot;

    private bool _resetCam;

    [SerializeField] private float _sensitivity;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _slowSpeed;
    [SerializeField] private float _normalSpeed;
    [SerializeField] [ReadOnly] private float _speed;

    void Start()
    {
        _defaultCameraPos = Camera.main.transform.position;
        _defaultCameraRot = Camera.main.transform.rotation;
    }


    void Update()
    {
        if (GameManager.Instance.FreeCam)
        {
            Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            Camera.main.transform.Rotate(mouseInput * _sensitivity * Time.deltaTime * 50f);
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            Camera.main.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speed = _sprintSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                _speed = _slowSpeed;
            }
            else
            {
                _speed = _normalSpeed;
            }
            transform.Translate(input * _speed * Time.deltaTime);
            GameManager.Instance.Player.GetComponent<PlayerMovement>().CanMove = false;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().CanJump = false;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().CanBlock = false;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().DodgeToggle = false;
            GameManager.Instance.Player.GetComponent<PlayerAttack>().CanLightAttack = false;
            GameManager.Instance.Player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
        }
        else
        {
            Camera.main.transform.position = _defaultCameraPos;
            Camera.main.transform.rotation = _defaultCameraRot;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().CanMove = true;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().CanJump = true;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().CanBlock = true;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().DodgeToggle = true;
            GameManager.Instance.Player.GetComponent<PlayerAttack>().CanLightAttack = true;
            GameManager.Instance.Player.GetComponent<PlayerAttack>().CanHeavyAttack = true;
        }
    }
}
