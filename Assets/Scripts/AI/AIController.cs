using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum AIState
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    DIE,
    DEAD
};

public class AIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetObject;
    [SerializeField]
    private float _distanceToChase = 5.0f;
    [SerializeField]
    private float _distanceToAttack = 0.5f;
    [SerializeField]
    private float _deathAnimationTime = 2.0f;
    [SerializeField]
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private Animator _animator;

    private bool _beginDeath = false;
    private AIState _state;
    private AIState _prevState;

    [SerializeField]
    private GameObject _dummy;
    [SerializeField]
    private string[] _animations;
    [SerializeField]
    private GameObject[] _patrolPoints;
    private int _currentPatrolPoint = -1;

    private bool _canPatrol = false;
    private bool _isPatrolling = false;

    private GameObject _target;

    private Coroutine _detectPlayer;
    private Collider _playerCollider;
    [SerializeField]
    private float _detectionDelay = 0.5f;
    private SphereCollider _detectionCollider;
    private void Awake()
    {
        _state = AIState.IDLE;
        _prevState = AIState.IDLE;

        int activePatrolPoints = 0;
        foreach(GameObject patrolPoint in _patrolPoints)
        {
            if(patrolPoint != null && patrolPoint.activeInHierarchy)
            {
                activePatrolPoints++;
            }
        }

        _currentPatrolPoint = 0;
        _canPatrol = activePatrolPoints > 1;

        _detectionCollider = this._dummy.GetComponent<SphereCollider>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        PerformAIActions();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(_state == AIState.DEAD)
        {
            return;
        }

        if(_beginDeath)
        {
            _state = AIState.DIE;
        }
        else
        {
            _state = AIState.IDLE;

            if(_canPatrol && _isPatrolling)
            {
                _state = AIState.PATROL;
            }
            if(IsWithinDistance(_targetObject, _distanceToChase))
            {
                _state = AIState.CHASE;
            }
            if(IsWithinDistance(_targetObject, _distanceToAttack))
            {
                _state = AIState.ATTACK;
            }
        }

        PerformAIActions();
    }

    private void PerformAIActions()
    {
        switch(_state)
        {
            case AIState.IDLE:
                Idle();
                break;
            case AIState.PATROL:
                Patrol();
                break;
            case AIState.CHASE:
                Chase();
                break;
            case AIState.ATTACK:
                Attack();
                break;
            case AIState.DIE:
                Die();
                break;
            default:
                break;
        }
/*
        if(_state != _prevState)
        {
            string animation = _animations[(int)_state];
            if(!string.IsNullOrEmpty(animation))
            {
                _prevState = _state;
            }
        }*/
    }

    private bool IsWithinDistance(GameObject gameObject, float distanceCheck)
    {
        if(gameObject == null)
        {
            return false;
        }
        
        float distance = Vector3.Distance(_dummy.transform.position, gameObject.transform.position);
        return distance < distanceCheck; 
    }

    private void Idle()
    {
        _navMeshAgent.SetDestination(_dummy.transform.position);
        _animator?.SetBool("IsWalking", false);
        StartCoroutine(StartPatrol());
    }
    
    IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(5.0f);
        _isPatrolling = true;
    }
    private void Chase()
    {
        SetDestination(_targetObject);
    }

    private void Patrol()
    {
        if(_currentPatrolPoint < 0)
        {
            return;
        }

        _animator?.SetBool("IsWalking", true);
        SetDestination(_patrolPoints[_currentPatrolPoint]);
        if(IsWithinDistance(_patrolPoints[_currentPatrolPoint], 1f))
        {
            _currentPatrolPoint++;
            _currentPatrolPoint %= _patrolPoints.Length;
            _isPatrolling = false;
            _state = AIState.IDLE;
            _navMeshAgent.SetDestination(_navMeshAgent.gameObject.transform.position);
        }
    }

    private void Attack()
    {

    }

    private void Die()
    {
        SetDestination(_dummy.gameObject);
        Invoke("HideEnemy", _deathAnimationTime);
    }

    private void SetDestination(GameObject destinationObject)
    {
        _navMeshAgent.destination = destinationObject.transform.position;
    }

    public void TakeDamage()
    {
        Debug.Log("AIController::TakeDamage()");
    }
}
