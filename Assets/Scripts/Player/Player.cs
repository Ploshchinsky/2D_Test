using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR.Haptics;

public class Player : MonoBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Run,
        BendDown
    }

    [SerializeField] private float _bendDownSpeed= 1.8f;
    [SerializeField] private float _walkingSpeed = 3.0f;
    [SerializeField] private float _runningSpeed = 4.2f;


    private Dictionary<State, float> _statesSpeed;
    private State _currentState;
    private Vector2 _inputVector = Vector2.zero;

    private Rigidbody2D _rb;
    private GameInput _gameInput;
    private ActiveWeapon _activeWeapon;

    public State CurrentState { get { return _currentState; } }
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        StateSpeedsInit();
    }

    private void Start()
    {
        _currentState = State.Idle;
        _activeWeapon = ActiveWeapon.Instance;
        _gameInput = GameInput.Instance;
        _gameInput.OnPlayerAttack += GameInput_OnPlayerAttack;
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        Sword weapon = _activeWeapon.getActiveWeapon();
        weapon.Attack();
    }

    private void Update()
    {
        UpdateInputDataForPlayerPosition();
    }


    private void FixedUpdate()
    {
        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {

        _rb.MovePosition(_rb.position + _inputVector * (Time.fixedDeltaTime * _statesSpeed.GetValueOrDefault(_currentState)));
    }

    private void UpdateInputDataForPlayerPosition()
    {
        _inputVector = _gameInput.GetMovementVector();
        _currentState = GetCurrentState(_gameInput);
    }

    public bool CheckIsWalking(float x, float y)
    {
        if (x != 0.0f || y != 0.0f)
        {
            return true;
        }
        return false;
    }

    private State GetCurrentState(GameInput gameInput)
    {
        if (gameInput.isBendDown()) return State.BendDown;
        if (gameInput.isFastRunning()) return State.Run;
        if (CheckIsWalking(_inputVector.x, _inputVector.y)) return State.Walk;

        return State.Idle;
    }
    private void StateSpeedsInit()
    {
        _statesSpeed = new Dictionary<State, float>();
        _statesSpeed.Add(State.BendDown, _bendDownSpeed);
        _statesSpeed.Add(State.Run, _runningSpeed);
        _statesSpeed.Add(State.Walk, _walkingSpeed);
    }

}
