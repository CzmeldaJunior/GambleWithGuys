using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _characterController;

    private PlayerInput _playerInput;

    [SerializeField]
    private Transform _cameraHolder;

    [SerializeField]
    private float _speed;

    private Vector2 _moveInput;

    private Vector2 _lookInput;

    [SerializeField]
    private float _sensitivity;

    private float _xRotation;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner)
            return;

        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!IsOwner)
            return;

        _lookInput = context.ReadValue<Vector2>();
    }

    private void MovementHandler()
    {
        Vector3 move = transform.right * _moveInput.x
                     + transform.forward * _moveInput.y;

        _characterController.Move(move * _speed * Time.deltaTime);
    }

    private void LookHandler()
    {
        float mouseX = _lookInput.x * _sensitivity;
        float mouseY = _lookInput.y * _sensitivity;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraHolder.localRotation =
            Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        MovementHandler();

        LookHandler();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"Player spawned | OwnerClientId: {OwnerClientId} | LocalClientId: {NetworkManager.LocalClientId} | IsOwner: {IsOwner}");

        if (!IsOwner)
        {
            _cameraHolder.gameObject.SetActive(false);
            _playerInput.enabled = false;
            return;
        }

        _cameraHolder.gameObject.SetActive(true);
        _playerInput.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
