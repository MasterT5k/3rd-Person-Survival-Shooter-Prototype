using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _jumpHeight = 10f;
    [SerializeField]
    private float _gravity = 20f;

    [Header("Camera Settings")]
    [SerializeField]
    private float _lookUpLimit = 0f;
    [SerializeField]
    private float _lookDownLimit = 26f;
    [SerializeField]
    private float _mouseYSensitivty = 2f, _mouseXSensitivty = 2f;

    private CharacterController _controller;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _yVelocity;
    private Camera _mainCamera;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (_controller == null)
        {
            Debug.LogError("Character Controller is NULL");
        }

        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera is NULL");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CalculateMovement();
        CameraController();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

    private void CalculateMovement()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        _direction = new Vector3(hInput, 0, vInput);
        _velocity = _direction * _speed;

        if (_controller.isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
            }
        }

        _yVelocity -= _gravity * Time.deltaTime;

        _velocity.y = _yVelocity;

        _velocity = transform.TransformDirection(_velocity);
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void CameraController()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += mouseX * _mouseXSensitivty;
        transform.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);

        Vector3 currentCameraRotation = _mainCamera.transform.localEulerAngles;
        currentCameraRotation.x -= mouseY * _mouseYSensitivty;
        currentCameraRotation.x = Mathf.Clamp(currentCameraRotation.x, _lookUpLimit, _lookDownLimit);
        _mainCamera.transform.localRotation = Quaternion.AngleAxis(currentCameraRotation.x, Vector3.right);
    }

    private void OnDisable()
    {
        _mainCamera.transform.parent = null;
    }
}
