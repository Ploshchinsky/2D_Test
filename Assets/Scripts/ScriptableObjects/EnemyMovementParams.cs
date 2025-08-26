using UnityEngine;

[CreateAssetMenu()]
public class EnemyMovementParams : ScriptableObject
{
    //Roaming Settings
    public bool isRoamingEnemy;
    public float RoamingDistanceMax;
    public float RoamingDistanceMin;
    public float RoamingTimerMax;

    //Chasing Settings
    public bool IsChasingEnemy;
    public float ChasingDistance;
    public float ChasingSpeedFactor;

    //Atack Settings
    public bool IsAttackingEnemy;
    public float AttackDistance;
    public float AttackCooldown;
}
