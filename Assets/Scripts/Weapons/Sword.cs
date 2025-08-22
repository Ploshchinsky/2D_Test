using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwordAttack;

   public void Attack()
    {
        OnSwordAttack.Invoke(this, EventArgs.Empty);
    }
}
