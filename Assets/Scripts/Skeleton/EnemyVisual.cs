using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private const string IS_MOVING = "isMoving";

    [SerializeField] private EnemyAI _enemyAI;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_MOVING, _enemyAI.isMoving());
    }

}
