using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerController _playerController;
    private CameraController _cameraController;

    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerController = GetComponent<PlayerController>();
        _cameraController = FindObjectOfType<CameraController>();

        if(_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.PlayerMovement.Movement.performed += i => _playerController.HandleMovementInput(i.ReadValue<Vector2>());
            _playerInput.PlayerActions.Sprint.performed += i => _playerController.HandleSprintInput(true);
            _playerInput.PlayerActions.Sprint.canceled += i => _playerController.HandleSprintInput(false);
            _playerInput.PlayerActions.Jump.started += i => _playerController.HandleJumpInput();


            _playerInput.PlayerMovement.Camera.performed += i => _cameraController.RotateCamera(i.ReadValue<Vector2>());

        }

        _playerInput.Enable();
    }

    private void Update()
    {

    }
}