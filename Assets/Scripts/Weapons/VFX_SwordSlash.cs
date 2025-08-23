using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_SwordSlash : MonoBehaviour
{
    private const string SLASH_TRIGGER = "SlashTrigger";

    [SerializeField] private Sword _sword;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _sword.OnSwordAttack += SwordVisual_onSwordAttack;
    }

    private void SwordVisual_onSwordAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(SLASH_TRIGGER);
    }
}
