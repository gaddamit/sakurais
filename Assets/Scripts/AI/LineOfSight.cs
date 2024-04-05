using System.Collections;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField]
    private float _defaultAngleDetection = 60.0f;
    [SerializeField]
    private float _angleDetection = 60.0f;
    public float AngleDetection
    {
        get { return _angleDetection; }
        set { _angleDetection = value; }
    }

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _detectionDelay = 0.5f;

    private Collider _targetCollider;
    private SphereCollider _detectionCollider;
    private Bounds _playerBounds;
    private Coroutine _detectPlayerCoroutine;

    private void Awake()
    {
        _detectionCollider = this.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.tag == "Player" )
        {
            _target = other.gameObject;
            _detectPlayerCoroutine = StartCoroutine( DetectPlayer() );
            _targetCollider = other.gameObject.GetComponent<CapsuleCollider>();
        }
    }

    private void OnTriggerExit( Collider other )
    {
        if ( other.tag == "Player" )
        {
            _target = null;
            StopCoroutine( _detectPlayerCoroutine );
        }
    }

    // Detects if the player is in the line of sight of the AI
    IEnumerator DetectPlayer()
    {
        while ( _target != null )
        {
            yield return new WaitForSeconds( _detectionDelay );

            Vector3[] points = GetBoundingPoints( _targetCollider.bounds );

            int points_hidden = 0;

            foreach ( Vector3 point in points )
            {
                Vector3 target_direction = point - this.transform.position;
                float target_distance = Vector3.Distance( this.transform.position, point );
                float target_angle = Vector3.Angle( target_direction, this.transform.forward );

                if ( IsPointCovered( target_direction, target_distance ) || target_angle > _angleDetection )
                    ++points_hidden;
            }

            if ( points_hidden >= points.Length )
            {
                if(_target)
                {
                    PlayerController playerController = _target.GetComponent<PlayerController>();
                    playerController.IsDetected = false;


                    AIController aiController = this.GetComponent<AIController>();
                    aiController.TargetObject = playerController;
                    aiController.StopChasing();
                }
            }
            else
            {
                if(_target)
                {
                    PlayerController playerController = _target.GetComponent<PlayerController>();
                    playerController.IsDetected = true;

                    AIController aiController = this.GetComponent<AIController>();
                    if(!aiController.IsAlreadyDead())
                    {
                        aiController.StartChasing(playerController);
                    }
                }
            }
        }
    }
    
    // Checks if a point is covered by an object
    // If the point is covered, the player is not in the line of sight
    private bool IsPointCovered( Vector3 target_direction, float target_distance )
    {
        RaycastHit[] hits = Physics.RaycastAll( this.transform.position, target_direction, _detectionCollider.radius );

        foreach ( RaycastHit hit in hits )
        {
            if ( hit.transform.gameObject.layer == LayerMask.NameToLayer( "Cover" ) || hit.transform.gameObject.layer == LayerMask.NameToLayer( "Ground" ) )
            {
                float cover_distance = Vector3.Distance( this.transform.position, hit.point );

                if ( cover_distance < target_distance )
                {
                    return true;
                }
            }
        }

        return false;
    }


    // Returns the 8 bounding points of a collider
    private Vector3[] GetBoundingPoints( Bounds bounds )
    {
        Vector3[] bounding_points =
        {
            bounds.min,
            bounds.max,
            new Vector3( bounds.min.x, bounds.min.y, bounds.max.z ),
            new Vector3( bounds.min.x, bounds.max.y, bounds.min.z ),
            new Vector3( bounds.max.x, bounds.min.y, bounds.min.z ),
            new Vector3( bounds.min.x, bounds.max.y, bounds.max.z ),
            new Vector3( bounds.max.x, bounds.min.y, bounds.max.z ),
            new Vector3( bounds.max.x, bounds.max.y, bounds.min.z )
        };

        return bounding_points;
    }

    // Resets the angle of detection to the default value
    public void ResetAngleOfDetection()
    {
        _angleDetection = _defaultAngleDetection;
    }
}