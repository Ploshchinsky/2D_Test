using System;
using UnityEngine;
using UnityEngine.AI;
using UtilsCollection;

[RequireComponent(typeof(NavMeshAgent), typeof(BoxCollider2D), typeof(CapsuleCollider2D))]
public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    [SerializeField] private State _startState = State.Roaming;
    private State _currentState;

    [SerializeField] private EnemyMovementParams _movementsParams;

    //Roaming Settings
    private float _currentRoamingTimer;
    private float _currentMovingSpeed;
    private Vector3 _targetRoamingPosition;

    //Chasing Settings
    private float _chasingSpeed;

    //Atack Settings
    private float _attackCooldownTimer = 0f;

    private NavMeshAgent _navMeshAgent;
    private BoxCollider2D _boxCollider2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private Player _player;

    public event EventHandler OnEnemyAttack;
    public event EventHandler OneEnemyDeath;

    public float ChasingSpeedFactor { get => _movementsParams.ChasingSpeedFactor; }

    private void Awake()
    {
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _currentState = _startState;
        _currentRoamingTimer = _movementsParams.RoamingTimerMax;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _currentMovingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _currentMovingSpeed * _movementsParams.ChasingSpeedFactor;
    }

    private void Start()
    {
        _player = Player.Instance;
    }

    private void Update()
    {
        StateHandler();
        UpdateFacingDirection();
    }


    public bool isMoving()
    {
        return _navMeshAgent.velocity != Vector3.zero;
    }

    public bool isAttacking()
    {
        return _currentState == State.Attacking;
    }

    public void setDeath()
    {
        OneEnemyDeath.Invoke(this, EventArgs.Empty);
        UpdateDeathState();
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Idle:
                UpdateIdleState();
                break;
            case State.Roaming:
                UpdateRoamingState();
                break;
            case State.Chasing:
                UpdateChasingState();
                break;
            case State.Attacking:
                UpdateAttackingState();
                break;
            case State.Death:
                UpdateDeathState();
                break;
        }
    }

    private void UpdateIdleState()
    {
        if (_movementsParams.isRoamingEnemy)
        {
            _currentState = State.Roaming;
            return;
        }
    }

    private void UpdateRoamingState()
    {
        // Логика блуждания
        _currentRoamingTimer += Time.deltaTime;
        if (_currentRoamingTimer >= _movementsParams.RoamingTimerMax)
        {
            _currentRoamingTimer = 0;
            Roaming();
        }

        // Переход из Roaming в Chasing
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        if (_movementsParams.IsChasingEnemy && distanceToPlayer <= _movementsParams.ChasingDistance)
        {
            _currentState = State.Chasing;
            Debug.Log($"Enemy START chasing player! Distance={distanceToPlayer}");
            return;
        }
    }

    private void UpdateChasingState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // Преследование игрока
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.speed = _chasingSpeed;

        // Переход из Chasing в Attacking
        if (_movementsParams.IsAttackingEnemy && distanceToPlayer <= _movementsParams.AttackDistance)
        {
            _currentState = State.Attacking;
            _attackCooldownTimer = _movementsParams.AttackCooldown;
            Debug.Log($"Enemy START attacking! Distance={distanceToPlayer}");
            return;
        }

        // Переход из Chasing в Roaming
        if (distanceToPlayer > _movementsParams.ChasingDistance)
        {
            _currentState = State.Roaming;
            _navMeshAgent.ResetPath();
            Debug.Log($"Enemy STOP chasing player! Distance={distanceToPlayer}");
            return;
        }
    }

    private void UpdateAttackingState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // Остановиться при атаке
        _navMeshAgent.ResetPath();

        // КД атаки
        _attackCooldownTimer += Time.deltaTime;
        if (_attackCooldownTimer >= _movementsParams.AttackCooldown)
        {
            _attackCooldownTimer = 0;
            AttackPlayer();
        }

        // Переход из Attacking в Chasing
        if (distanceToPlayer > _movementsParams.AttackDistance)
        {
            _currentState = State.Chasing;
            Debug.Log($"Enemy STOP attacking! Distance={distanceToPlayer}");
            return;
        }
    }

    private void AttackPlayer()
    {
        // Логика атаки
        _currentState = State.Attacking;
        OnEnemyAttack.Invoke(this, EventArgs.Empty);
    }

    private void UpdateDeathState()
    {
        // Логика смерти
        _boxCollider2D.enabled = false;
        _capsuleCollider2D.enabled = false;
        _navMeshAgent.ResetPath();
        _currentState = State.Death;

    }

    private void Roaming()
    {
        float randomDistance = UnityEngine.Random.Range(_movementsParams.RoamingDistanceMin, _movementsParams.RoamingDistanceMax);
        _targetRoamingPosition = transform.position + (Utils.GetRandomDirection() * randomDistance);
        _navMeshAgent.SetDestination(_targetRoamingPosition);
    }

    private void UpdateFacingDirection()
    {
        if (_navMeshAgent.hasPath && _navMeshAgent.remainingDistance > 0.1f)
        {
            // Получаем следующую точку на пути
            Vector3 nextPathPoint = _navMeshAgent.steeringTarget;

            // Определяем направление к следующей точке
            Vector3 directionToNextPoint = (nextPathPoint - transform.position).normalized;

            ChangeFacingDirection(directionToNextPoint);
        }
    }

    private void ChangeFacingDirection(Vector3 targetDirection)
    {
        // Проекция на плоскость XZ для 3D или использование только X для 2D-style
        Vector3 flattenedDirection = new Vector3(targetDirection.x, 0, targetDirection.z).normalized;

        if (flattenedDirection.magnitude > 0.1f)
        {
            // Для 2D-style (только по X)
            if (Mathf.Abs(flattenedDirection.x) > 0.5f)
            {
                bool shouldFaceLeft = flattenedDirection.x < 0;
                transform.rotation = Quaternion.Euler(0, shouldFaceLeft ? 180f : 0f, 0);
            }
        }
    }

}
