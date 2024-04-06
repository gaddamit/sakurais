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

    public Vector2 _move;
    public Vector2 _look;

    [Header("Flags")]
    private bool _allowAiming = false;
    private bool _isAiming = false;
    private bool _isThrowing = false;
    private bool _isCrouching = false;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private bool _isSprinting = false;
    private bool _isDetected = false;
    private bool _isStabbing = false;
    private bool _allowStabbing = false;

    [SerializeField]
    private GameObject _stabCollider;
    public bool IsDetected
    {
        get { return _isDetected; }
        set { _isDetected = value; }
    }

    [Header("Physics")]
    private Rigidbody _rigidbody;

    [Header("Jump info")]
    [SerializeField]
    private float _jumpForce = 20f;

    private AnimatorController _animatorController;
    private Vector3 _movementDirection;
    private Transform _cameraTransform;

    [Header("Projectile")]
    [SerializeField]
    private GameObject _currentProjectile;
    [SerializeField]
    private GameObject _projectileSpawnPoint;

    [SerializeField]
    private GameObject _followTarget;
    [SerializeField]
    private GameObject _aimTarget;

    [Header("Camera")]
    [SerializeField]
    private float _cameraRotationSpeed = 2f;

    private PlayerAimController _playerAimController;
    
    private Vector3 _nextPosition;
    private Quaternion _nextRotation;



    private PlayerHealth _playerHealth;
    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
        _rigidbody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        _playerAimController = GetComponent<PlayerAimController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.onPlayerDeathDelegate += StartDeath;

        Invoke("AllowInput", 1.0f);
    }

    private void AllowInput()
    {
        _allowAiming = true;
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
        if(_isAiming || _isThrowing || _isStabbing || _playerHealth.IsDead) 
        {
            _xMovement = 0;
            _yMovement = 0;
            return;
        }

        Vector3 moveDirection = _cameraTransform.forward * _yMovement;
        moveDirection += _cameraTransform.right * _xMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (_isSprinting)
        {
            moveDirection = moveDirection * _sprintSpeed;
        }
        else if (_isCrouching)
        {
            moveDirection = moveDirection * _walkSpeed;
        }
        else
        {
            moveDirection = moveDirection * _runSpeed;
        }

        moveDirection.y = _rigidbody.velocity.y;
        //Debug.Log(moveDirection);
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
        _followTarget.transform.localRotation =  Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void RotateCamera(Vector2 lookInput)
    {
        _look = lookInput;
    }

    private void LateUpdate()
    {
         if (_move == Vector2.zero) 
        {   
            if (_isAiming)
            {
                //Set the player rotation based on the look transform
                transform.rotation = Camera.main.transform.rotation;
            }

            return; 
        }
    }

    private Vector3 ClampCameraRotation()
    {
        Vector3 angles = _followTarget.transform.localEulerAngles;
        angles.z = 0;

        float angle = _followTarget.transform.localEulerAngles.x;

        // Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        return angles;
    }

    public void HandleMovementInput(Vector2 movementInput)
    {
        if(_isAiming || _isThrowing || _isStabbing || _playerHealth.IsDead) 
        {
            return;
        }

        _xMovement = movementInput.x;
        _yMovement = movementInput.y;
        
        _movementAmount = Mathf.Abs(_xMovement) + Mathf.Abs(_yMovement);
    }

    public void HandleSprintInput(bool sprinting)
    {
        _isSprinting = sprinting && _movementAmount > 0.5f;
    }

    public void HandleJumpInput()
    {
        if(_isGrounded && !_isJumping && !_isCrouching && !_isAiming && !_isThrowing && !_isStabbing)
        {
            _isJumping = true;
            _isGrounded = false;

            Vector3 velocity = _rigidbody.velocity;
            velocity.y = _jumpForce;
            _rigidbody.velocity = velocity;

            _animatorController.SetTrigger("Jump");
            _animatorController.SetAnimationParameter("IsGrounded", false);
        }
    }

    public void HandleThrowInput(bool aiming)
    {
        if(!_allowAiming)
        {
            return;
        }

        if(!_isCrouching && !_isAiming && !_isThrowing)
        {
            _rigidbody.velocity = Vector3.zero;

            _isAiming = true;
            _animatorController.SetAnimationParameter("IsAiming", _isAiming);
            _playerAimController.StartAiming();
        }
        else if(_isAiming && !aiming)
        {
            _isAiming = false;
            _animatorController.SetAnimationParameter("IsAiming", _isAiming);

            _isThrowing = true;
            _animatorController.SetAnimationParameter("IsThrowing", _isThrowing);

            Invoke("PerformThrow", 0.25f);
            Invoke("ResetThrowing", 1.0f);
        }
    }

    public void HandleCrouchInput(bool crouching)
    {
        _isCrouching = crouching;
        _animatorController.SetAnimationParameter("IsCrouching", _isCrouching);
    }

    private void PerformThrow()
    {
        //Instantiate the projectile and throw it
        GameObject projectile = Instantiate(_currentProjectile, _projectileSpawnPoint.transform.position, _currentProjectile.transform.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>(); 
        projectileScript.Throw(_projectileSpawnPoint.transform);
    }

    private void ResetThrowing()
    {
        _isThrowing = false;

        _animatorController.SetAnimationParameter("IsThrowing", false);
        _animatorController.UpdateMovementValues(0, 0, false); 
        _playerAimController.StopAiming();

        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0);
    }

    public void HandleStabInput()
    {
        if(!_isStabbing && !_isJumping && !_isAiming && !_isThrowing)
        {
            _rigidbody.velocity = Vector3.zero;
            _isStabbing = true;
            _animatorController.SetTrigger("Stab");
            _allowStabbing = false;
        }
    }

    // Called from the animation event from the player's stab animation
    private void Attack()
    {
        _stabCollider.SetActive(true);
    }

    // Called from the animation event from the player's stab animation
    private void FinishAttack()
    {
        _animatorController.ResetTrigger("Stab");
        _stabCollider.SetActive(false);
        _isStabbing = false;
        _isDetected = false;
    }

    // Handle ground detection
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = true;
            _isJumping = false;
            _animatorController.SetAnimationParameter("IsGrounded", true);

            _animatorController.ResetTrigger("Jump");
        }

        CapsuleCollider collider = other.GetComponent<CapsuleCollider>();
        if(other.isTrigger && other.GetType() == typeof(CapsuleCollider))
        {
            if(other.gameObject.tag == "Enemy" && !_isDetected)
            {
                _allowStabbing = true;
            }
        }
    }

    // Handle ground detection
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = false;
        }
        if(other.gameObject.tag == "Enemy")
        {
            _allowStabbing = false;
        }
    }

    private void StartDeath()
    {
        _animatorController.SetTrigger("Dying");
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }

    public bool IsAlreadyDead()
    {
        return _playerHealth.IsDead;
    }
}