using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is responsible for handling the player's level, such as climbing stairs
public class PlayerLevelController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Step Climb")]
    [SerializeField]
    private bool _enableStepClimb = true;
    [SerializeField]
    private GameObject _stepRayUpper;
    [SerializeField]
    private float _stepRayUpperLength = 0.2f;
    [SerializeField]
    private GameObject _stepRayLower;
    [SerializeField]
    private float _stepRayLowerLength = 0.1f;
    [SerializeField]
    private float _stepHeight = 0.3f;
    [SerializeField]
    private float _stepSmooth = 0.01f;
    [SerializeField]
    private bool _debugStepClimbRays = true;

    [Header("Grounding")]
    [SerializeField]
    private float _minGroundNormalY = 0.65f;
    [SerializeField, Min(0f)]
    private float _maxDistanceToCastRay = 1f;
    [SerializeField]
    private float _groundingDamper = 0.5f;

    private Vector3 _contactNormal;
    private int _groundContactCount = 0;
    private int _stepsSinceLastGrounded = 0;
    private PlayerController _playerController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //Set the step ray positions
        _stepRayUpper.transform.position = new Vector3(_stepRayUpper.transform.position.x, _stepHeight, _stepRayUpper.transform.position.z);

        //Get the player controller
        _playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _stepsSinceLastGrounded++;
        
        if(!_playerController.IsAiming || !_playerController.IsThrowing)
        {
            HandleStepClimb();
        }

        if(_playerController.IsGrounded || SnapToGround())
        {
            _stepsSinceLastGrounded = 0;
            if(_groundContactCount > 1)
            {
                _contactNormal.Normalize();
            }
        }
        else
        {
            _contactNormal = Vector3.up;
        }
    }

    // Prevent rigidbody from flying off when the player climbs a slope
    // because of the player's velocity
    private bool SnapToGround()
    {
        if(_stepsSinceLastGrounded > 1)
        {
            return false;
        }

        if(!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _maxDistanceToCastRay))
        {
            return false;
        }

        if(hit.normal.y < _minGroundNormalY)
        {
            return false;
        }

        _groundContactCount = 1;
        _contactNormal = hit.normal;

        float speed = _rigidbody.velocity.magnitude;
        float dot = Vector3.Dot(_rigidbody.velocity, hit.normal) * _groundingDamper;
        if(dot > 0f)
        {
            _rigidbody.velocity = (_rigidbody.velocity - hit.normal * dot).normalized * speed;
        }

        return true;
    }

    //Casts two rays forward to check if the player can step up a stair
    //If the lower ray hits something and the upper ray doesn't, the player is allowed to step up
    private void HandleStepClimb()
    {
        if(!_enableStepClimb)
        {
            return;
        }

        RaycastHit hitLower;

        
        if(Physics.Raycast(_stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, _stepRayLowerLength))
        {
            RaycastHit hitUpper;
            Color color = Color.green;
            
            if(!Physics.Raycast(_stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, _stepRayUpperLength))
            {
                _rigidbody.position -= new Vector3(0, -_stepSmooth, 0);
                color = Color.red;
            }

            if(_debugStepClimbRays)
            {
                Debug.DrawRay(_stepRayLower.transform.position, transform.TransformDirection(Vector3.forward) * _stepRayLowerLength, Color.green);
                Debug.DrawRay(_stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward) * _stepRayUpperLength, color);
            }
        }
        else if(_debugStepClimbRays)
        {
            Debug.DrawRay(_stepRayLower.transform.position, transform.TransformDirection(Vector3.forward) * _stepRayLowerLength, Color.red);
        }

        RaycastHit hitLower45;
        if(Physics.Raycast(_stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1), out hitLower45, _stepRayLowerLength))
        {
            RaycastHit hitUpper45;
            if(!Physics.Raycast(_stepRayUpper.transform.position, transform.TransformDirection(1.5f,0,1), out hitUpper45, _stepRayUpperLength))
            {
                _rigidbody.position -= new Vector3(0, -_stepSmooth, 0);
            }
        }

        RaycastHit hitLowerMinus45;
        if(Physics.Raycast(_stepRayLower.transform.position, transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, _stepRayLowerLength))
        {
            RaycastHit hitUpperMinus45;
            if(!Physics.Raycast(_stepRayUpper.transform.position, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, _stepRayUpperLength))
            {
                _rigidbody.position -= new Vector3(0, -_stepSmooth, 0);
            }
        }
    }
}
