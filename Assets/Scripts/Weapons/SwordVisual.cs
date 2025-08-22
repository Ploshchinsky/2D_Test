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

    private void Awake()
    {
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

}
