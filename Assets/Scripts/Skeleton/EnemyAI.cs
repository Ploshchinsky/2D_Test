using System;
using UnityEngine;
using UnityEngine.AI;
using UtilsCollection;
public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attack,
        Death
    }

    [SerializeField] private State _startState = State.Roaming;

    //Roaming Settings
    [SerializeField] private float _roamingDistanceMax = 8.0f;
    [SerializeField] private float _roamingDistanceMin = 1.0f;
    [SerializeField] private float _roamingTimerMax = 2.0f;

    //Chasing Settings
    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedFactor = 2f;

    private float _movingSpeed;
    private float _chasingSpeed;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private Vector3 _targetRoamingPosition;
    private float _currentRoamingTimer;

    private void Awake()
    {
        _currentState = _startState;
        _currentRoamingTimer = 0;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _movingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _movingSpeed * _chasingSpeedFactor;
    }
    private void Update()
    {
        StateHandler();
    }

    private void StateHandler()
    {
        UpdateState();
        switch (_currentState)
        {
            default:
            case State.Roaming:
                _currentRoamingTimer += Time.deltaTime;
                if (_currentRoamingTimer >= _roamingTimerMax)
                {
                    Roaming();
                    _currentRoamingTimer = 0;
                }
                break;
            case State.Chasing:
                break;
            case State.Attack: break;
            case State.Death: break;
            case State.Idle: break;
        }
    }

    private void UpdateState()
    {
        if (_isChasingEnemy)
        {
            Player player = Player.Instance;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= _chasingDistance)
            {
                Debug.Log($"Enemy START chasing player! Distance={distanceToPlayer}");
                _navMeshAgent.ResetPath();
                _navMeshAgent.SetDestination(player.transform.position);
                _navMeshAgent.speed = _chasingSpeed;

                _currentState = State.Chasing;
                return;
            }

            if (distanceToPlayer > _chasingDistance && _currentState == State.Chasing)
            {
                Debug.Log($"Enemy STOP chasing player! Distance={distanceToPlayer}");
                _currentState = State.Roaming;
                _navMeshAgent.speed = _movingSpeed;
                _currentRoamingTimer = 0;
                return;
            }
            return;
        }

        if (!_isChasingEnemy)
        {
            _currentState = State.Roaming;
            return;
        }
    }

    private void Roaming()
    {
        float randomDistance = UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
        _targetRoamingPosition = transform.position + (Utils.GetRandomDirection() * randomDistance);
        ChangeFacingDirection(transform.position, _targetRoamingPosition);
        _navMeshAgent.SetDestination(_targetRoamingPosition);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        transform.rotation = Quaternion.Euler(0, sourcePosition.x > targetPosition.x ? -180 : 0, 0);
    }

    public bool isMoving()
    {
        return _navMeshAgent.velocity != Vector3.zero;
    }

}
