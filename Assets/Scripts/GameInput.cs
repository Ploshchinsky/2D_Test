using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Camera _cameraMain;
    private Player _player;

    public event EventHandler OnPlayerAttack;

    public static GameInput Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _player = Player.Instance;
        _cameraMain = Camera.main;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.Combat.Attack.started += PlayerAttack_started; ;

    }

    private void PlayerAttack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public bool isFastRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public bool isBendDown()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
}
