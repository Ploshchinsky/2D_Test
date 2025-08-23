using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageValue;

    public event EventHandler OnSwordAttack;
    private PolygonCollider2D _polygonCollider2D;

    public void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Debug.Log($"Sword find enemy {enemyEntity}");
            enemyEntity.TakeDamage(_damageValue);
        } else
        {
            Debug.Log("Sword not found enemy");
        }
    }

    public void Attack()
    {
        OnSwordAttack.Invoke(this, EventArgs.Empty);
        SwordColliderSwitch(true);
    }

    public void SwordColliderSwitch(bool switchOn)
    {
        _polygonCollider2D.enabled = switchOn;
    }

}
