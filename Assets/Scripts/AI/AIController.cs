using System.Collections;
using UnityEngine;
using UnityEngine.AI;

enum AIState
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    DYING,
    DEAD
};

public class AIController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _targetObject;
    public PlayerController TargetObject
    {
        get { return _targetObject; }
        set { _targetObject = value; }
    }

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

    PlayerHealth _playerHealth;
    [SerializeField]
    private Collider _bodyCollider;

    [SerializeField]
    private GameObject _alertObject;

    [SerializeField]
    private GameObject _stabCollider;
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

        _alertObject.SetActive(false);
    }

    // Start is called before the first frame update
    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.onPlayerDeathDelegate += StartDeath;

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
        if(_playerHealth.IsDead)
        {
            if(_state < AIState.DYING)
            {
                return;
            }
        }

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
                //Attack();
                break;
            case AIState.DYING:
                Dying();
                break;
            case AIState.DEAD:
                Dead();
                break;
            default:
                break;
        }
    }

    private bool IsWithinDistance(float distance)
    {
        return _navMeshAgent.remainingDistance <= distance;
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

    public void StartChasing(PlayerController playerController)
    {
        if(_state >= AIState.DYING || playerController != null && playerController.IsAlreadyDead())
        {
            StopChasing();
            return;
        }
        if(Vector3.Distance(transform.position, playerController.transform.position) > _distanceToChase)
        {
            StopChasing();
            return;
        }

        _targetObject = playerController;
        _alertObject.SetActive(true);
    
        _state = AIState.CHASE;
        _animator?.SetBool("IsWalking", true);
    }

    public void StopChasing()
    {
        if(_state >= AIState.DYING)
        {
            return;
        }

        _targetObject = null;
        _state = AIState.IDLE;
        _alertObject.SetActive(false);

        _animator?.SetBool("IsWalking", false);
        StopMovement();
    }

    private void Chase()
    {
        if(_targetObject != null)
        {
            SetDestination(_targetObject.gameObject);
        }
        
        if(IsWithinDistance(1) && !_navMeshAgent.pathPending)
        {
            _animator?.SetBool("IsWalking", false);
            StartAttack();
        }
    }

    private void Patrol()
    {
        if(IsWithinDistance(_navMeshAgent.stoppingDistance) && !_navMeshAgent.pathPending)
        {
            _currentPatrolPoint++;
            _currentPatrolPoint %= _patrolPoints.Length;
            
            _animator?.SetBool("IsWalking", false);
            _state = AIState.IDLE;
        }
    }

    private void StartAttack()
    {
        StopMovement();

        _state = AIState.ATTACK;
        transform.LookAt(_targetObject.transform);
        _animator?.SetTrigger("Stab");
    }

    private void Attack()
    {
        _stabCollider.SetActive(true);
    }

    private void FinishAttack()
    {
        _stabCollider.SetActive(false);

        if(_targetObject == null || _targetObject && _targetObject.IsAlreadyDead())
        {
            StopChasing();
        }
        else
        {
            StartChasing(_targetObject);
        }
    }


    private void Dying()
    {
    }

    private void SetDestination(GameObject destinationObject)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = destinationObject.transform.position;
    }

    private void StopMovement()
    {
        _navMeshAgent.SetDestination(transform.position);
        _navMeshAgent.isStopped = true;
    }

    public void TakeDamage()
    {
        Debug.Log("AIController::TakeDamage() " + gameObject.name);
    }

    private void StartDeath()
    {
        _animator.SetTrigger("Dying");
        _state = AIState.DYING;

        _alertObject.SetActive(false);
        _bodyCollider.enabled = false;
        StopMovement();
        Invoke("Dead", _deathAnimationTime);
    }

    private void Dead()
    {   
        _state = AIState.DEAD;
        transform.parent.gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
    }

    public bool IsAlreadyDead()
    {
        return _playerHealth.IsDead;
    }

    // Increase the angle of detection of the AI if it is undear a lamp
    public void SetAngleOfDetection(float value)
    {
        LineOfSight lineOfSight = GetComponent<LineOfSight>();
        if(lineOfSight != null)
        {
            lineOfSight.AngleDetection = value;
        }
    }

    // Reset the angle of detection of the AI to the default value
    public void ResetAngleOfDetection()
    {
        LineOfSight lineOfSight = GetComponent<LineOfSight>();
        if(lineOfSight != null)
        {
            lineOfSight.ResetAngleOfDetection();
        }
    }
}
