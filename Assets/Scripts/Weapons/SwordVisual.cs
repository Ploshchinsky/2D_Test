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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Debug.Log("Player's Sword find enemy!");
            enemyEntity.TakeDamage(_sword.DamageValue);
            return;
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

    private void SwordVisual_onSwordAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
        SwordColliderSwitch(true);
    }

}
