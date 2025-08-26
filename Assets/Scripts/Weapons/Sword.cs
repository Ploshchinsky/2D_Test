using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] public int DamageValue;

    public event EventHandler OnSwordAttack;

    public void Attack()
    {
        OnSwordAttack.Invoke(this, EventArgs.Empty);
    }

}
