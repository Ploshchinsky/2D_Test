using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string IS_RUNNING = "isRunning";
    private Animator _animator;
    private Camera _mainCamera;

    [SerializeField] private float _bendDownScaleY = 0.88f;
    [SerializeField] private float _normalScaleY = 1f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Player player = Player.Instance;
        GameInput gameInput = GameInput.Instance;

        HandleIsMovingState(player, gameInput);
        HandleBendDown(player);
    }

    private void HandleIsMovingState(Player player, GameInput gameInput)
    {
        Vector2 gameInputVector = gameInput.GetMovementVector();
        _animator.SetBool(IS_RUNNING, player.CheckIsWalking(gameInputVector.x, gameInputVector.y));
    }

    private void HandleBendDown(Player player)
    {
        this.transform.localScale = Player.State.BendDown.Equals(player.CurrentState)
                    ? new Vector3(transform.localScale.x, _bendDownScaleY, transform.localScale.z)
                    : new Vector3(transform.localScale.x, _normalScaleY, transform.localScale.z);
    }

    
}
