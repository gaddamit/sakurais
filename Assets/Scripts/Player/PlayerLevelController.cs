using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Step Climb")]
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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //Set the step ray positions
        _stepRayUpper.transform.position = new Vector3(_stepRayUpper.transform.position.x, _stepHeight, _stepRayUpper.transform.position.z);
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
        HandleStepClimb();
    }

    //Casts two rays forward to check if the player can step up a stair
    //If the lower ray hits something and the upper ray doesn't, the player is allowed to step up
    private void HandleStepClimb()
    {
        RaycastHit hitLower;
        if(_debugStepClimbRays)
        {
            Debug.DrawRay(_stepRayLower.transform.position, transform.TransformDirection(Vector3.forward) * _stepRayLowerLength, Color.red);
            Debug.DrawRay(_stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward) * _stepRayUpperLength, Color.red);
        }

        if(Physics.Raycast(_stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, _stepRayLowerLength))
        {
            RaycastHit hitUpper;
            if(!Physics.Raycast(_stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, _stepRayUpperLength))
            {
                _rigidbody.position -= new Vector3(0, -_stepSmooth, 0);
            }
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
