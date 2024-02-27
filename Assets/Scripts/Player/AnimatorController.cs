using System.Collections;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void UpdateMovementValues(float xMovement, float yMovement, bool isSprinting = false)
    {
        float snappedX = SnapValues(xMovement, 0.55f, 0.5f, 1.0f);
        float snappedY = SnapValues(yMovement, 0.55f, 0.5f, 1.0f);

        if (isSprinting)
        {
            snappedY = 2f;
        }

        _animator.SetFloat("XMovement", snappedX, .1f, Time.deltaTime);
        _animator.SetFloat("YMovement", snappedY, .1f, Time.deltaTime);
    }

    private float SnapValues(float value, float lowerBound, float lowValue, float highValue)
    {
        if( value > 0 && value < lowerBound)
        {
            return lowValue;
        }
        else if (value > lowerBound)
        {
            return highValue;
        }
        else if(value < 0 && value < -lowerBound)
        {
            return -lowValue;
        }
        else if(value < -lowerBound)
        {
            return -highValue;
        }
        
        return 0;
    }

    public void SetAnimationParameter(string parameterName, bool value)
    {
        _animator.SetBool(parameterName, value);
    }

    public void SetTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
}