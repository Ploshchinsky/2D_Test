using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    private const string ATTACK = "Attack";

    [SerializeField] Sword _sword;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _sword.OnSwordAttack += SwordVisual_onSwordAttack;
    }

    private void SwordVisual_onSwordAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
        SwordColliderSwitch(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Debug.Log($"Sword find enemy {enemyEntity}");
            enemyEntity.TakeDamage(_sword.DamageValue);
        }
        else
        {
            Debug.Log("Sword not found enemy");
        }
    }

    public void AE_OnAttackEnd(AnimationEvent e)
    {
        SwordColliderSwitch(false);
    }

    public void SwordColliderSwitch(bool switchOn)
    {
        _polygonCollider2D.enabled = switchOn;
    }

}
