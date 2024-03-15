using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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

    //Requires at least 2 patrol points assigned to be able to patrol
    private bool _canPatrol = false;

    [Header("Patrol")]
    [SerializeField]
    private float _patrolWait = 5.0f;
    [SerializeField]
    private GameObject[] _patrolPoints;
    private int _currentPatrolPoint = -1;
    private Coroutine _patrolCoroutine = null;

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

        _currentPatrolPoint = 1;
        _canPatrol = activePatrolPoints > 1;
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
    }

    private bool IsWithinDistance()
    {
        return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
    }

    private void Idle()
    {
        if(_canPatrol)
        {
            if(_patrolCoroutine == null)
            {
                _patrolCoroutine = StartCoroutine(StartPatrol());
            }
        }
    }
    
    IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(_patrolWait);

        _animator?.SetBool("IsWalking", true);
        SetDestination(_patrolPoints[_currentPatrolPoint]);
        _state = AIState.PATROL;

        _patrolCoroutine = null;
    }

    private void Chase()
    {
        SetDestination(_targetObject);
    }

    private void Patrol()
    {
        if(IsWithinDistance() && !_navMeshAgent.pathPending)
        {
            _currentPatrolPoint++;
            _currentPatrolPoint %= _patrolPoints.Length;
            
            _animator?.SetBool("IsWalking", false);
            _state = AIState.IDLE;
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
        Debug.Log("AIController::TakeDamage() " + gameObject.name);
    }
}
