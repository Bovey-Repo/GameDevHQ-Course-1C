using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]

    [SerializeField]
    private float _speed = 6.0f;
    [SerializeField]
    private float _jumpHeight = 8.0f;
    [SerializeField]
    private float _gravity = 20.0f;

    [Header("Camera Settings")]
    [SerializeField]
    private float _mouseSpeed = 5.0f;
    [SerializeField]
    private float _cameraVMin = -25.0f;
    [SerializeField]
    private float _cameraVMax = 90.0f;
    [SerializeField]
    private float _currLookRotation = 0.0f;

    private Vector3 _direction;
    private Vector3 _velocity;
    private CharacterController _charController;
    private Camera _mainCamera;
  
    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        if (_charController == null)
        {
            Debug.LogError("Player : Character Controller is NULL");
        }

        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("Player : Main Camera is NULL");
        }

        // lock & hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MoveViewpoint();
        MovePlayer();

        //if escape key pressed unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void MoveViewpoint()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // rotate the player left/right
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += (mouseX * _mouseSpeed);
        transform.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);

        // rotate the camera verticaly with the mouse
        // use "_currLookRotation +..." to invert mouseY NOTE: needs UI to switch boolean
        _currLookRotation = _currLookRotation - (mouseY * _mouseSpeed);
        _currLookRotation = Mathf.Clamp(_currLookRotation, _cameraVMin, _cameraVMax);
        _mainCamera.transform.localRotation = Quaternion.identity;
        _mainCamera.transform.Rotate(Vector3.left, _currLookRotation);
    }

    private void MovePlayer()
    {
        if (_charController.isGrounded == true)
        {
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");
            _direction = new Vector3(xInput, 0, zInput);
            _velocity = _direction * _speed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = _jumpHeight;
            }
        }

        _velocity.y -= _gravity * Time.deltaTime;

        //move the player in the in the world space direction they are facing in the local space 
        _velocity = transform.TransformDirection(_velocity);
        _charController.Move(_velocity * Time.deltaTime);
    }
}
