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
    private TrailRenderer _swordTrailParticles;

    private void Awake()
    {
        _swordTrailParticles = GetComponent<TrailRenderer>();
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
    }

    // Animation Event: начать след при атаке
    public void AE_OnAttackStart(AnimationEvent e)
    {
        _swordTrailParticles.Clear(); // Очищаем старые частицы
        _swordTrailParticles.emitting = true;
    }

    // Animation Event: остановить след
    public void AE_OnAttackEnd(AnimationEvent e)
    {
        _swordTrailParticles.emitting = false;
    }

}
