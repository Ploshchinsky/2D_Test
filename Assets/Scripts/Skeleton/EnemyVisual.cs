using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyVisual : MonoBehaviour
{
    private const string IS_MOVING = "isMoving";
    private const string IS_DEATH = "isDeath";
    private const string TAKE_HIT = "TakeHit";
    private const string ATTACK = "Attack";
    private const string CHASING_SPEED_FACTOR = "ChasingSpeedFactor";

    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyAI.OneEnemyDeath += _enemyAI_OneEnemyDeath;
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
    }

    private void _enemyAI_OneEnemyDeath(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IS_DEATH);
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(TAKE_HIT);
    }

    private void Update()
    {
        AnimationHandler();
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    // ========================================================= //

    public void OnAnimationAttack_On()
    {
        _enemyEntity.PolygonColliderSwitch(true);
    }

    public void OnAnimationAttack_Off()
    {
        _enemyEntity.PolygonColliderSwitch(false);
    }

    private void AnimationHandler()
    {
        _animator.SetBool(IS_MOVING, _enemyAI.isMoving());
        _animator.SetFloat(CHASING_SPEED_FACTOR, _enemyAI.ChasingSpeedFactor);
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }

}
