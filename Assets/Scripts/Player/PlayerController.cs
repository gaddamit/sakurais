using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _rotationSpeed = 15f;
    [SerializeField]
    private float _walkSpeed = 1.5f;
    [SerializeField]
    private float _runSpeed = 5f;
    [SerializeField]
    private float _sprintSpeed = 7f;

    [Header("Input")]
    private float _xMovement;
    private float _yMovement;
    private float _movementAmount;

    [Header("Flags")]
    private bool _isGrounded = true;
    private bool _isJumping;
    private bool _isSprinting;

    [Header("Physics")]
    private Rigidbody _rigidbody;

    [Header("Jump info")]
    [SerializeField]
    private float _jumpForce = 20f;

    private AnimatorController _animatorController;
    private Vector3 _movementDirection;
    private Transform _cameraTransform;

    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
        _rigidbody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _animatorController.UpdateMovementValues(_xMovement, _yMovement, _isSprinting);
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = _cameraTransform.forward * _yMovement;
        moveDirection += _cameraTransform.right * _xMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (_isSprinting)
        {
            moveDirection = moveDirection * _sprintSpeed;
        }
        else
        {
            if (_movementAmount >= 0.5f)
            {
                moveDirection = moveDirection * _runSpeed;
            }
            else
            {
                moveDirection = moveDirection * _walkSpeed;
            }
        }
        moveDirection.y = _rigidbody.velocity.y;
        _rigidbody.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = _cameraTransform.forward * _yMovement;
        targetDirection = targetDirection + _cameraTransform.right * _xMovement;
        targetDirection.Normalize();
        targetDirection.y = 0;
        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;
            
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleMovementInput(Vector2 movementInput)
    {
        _xMovement = movementInput.x;
        _yMovement = movementInput.y;
        
        _movementAmount = Mathf.Abs(_xMovement) + Mathf.Abs(_yMovement);
    }

    public void HandleSprintInput(bool sprinting)
    {
        _isSprinting = sprinting;
    }

    public void HandleJumpInput()
    {
        if(_isGrounded)
        {
            _isJumping = true;
            Vector3 velocity = _rigidbody.velocity;
            velocity.y = _jumpForce;
            _rigidbody.velocity = velocity;
            _isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = true;
            _isJumping = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = false;
        }
    }
}