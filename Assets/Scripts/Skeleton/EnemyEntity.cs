using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyEntity : MonoBehaviour
{
    private const string PLAYER_HITBOX = "PlayerHitbox";

    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyStats Stats;

    private int _currentHealth;

    public event EventHandler OnTakeHit;

    private PolygonCollider2D _poligonCollider2D;
    private Player _player;

    private void Awake()
    {
        _poligonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        _player = Player.Instance;

        _currentHealth = Stats.Health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (string.Equals(collision?.gameObject.name, PLAYER_HITBOX))
        {
            Debug.Log("Enemy Sword find player!");
            //_player.TakeDamage(HealthParams.Damage);
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        OnTakeHit.Invoke(this, EventArgs.Empty);
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _enemyAI.setDeath();
            return;
        }
    }
    public void PolygonColliderSwitch(bool switchOn)
    {
        _poligonCollider2D.enabled = switchOn;
    }

}
