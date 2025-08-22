using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public event EventHandler OnSwordAttack;

    private void FixedUpdate()
    {
        HandlePLayerFacingDirection(GameInput.Instance, Player.Instance);
    }

    public void Attack()
    {
        OnSwordAttack.Invoke(this, EventArgs.Empty);
    }

    private void HandlePLayerFacingDirection(GameInput gameInput, Player player)
    {
        Vector3 mouseVector = gameInput.GetMousePosition();
        Vector3 playerVector = Camera.main.WorldToScreenPoint(player.transform.position);

        float mouseX = mouseVector.x;
        float playerX = playerVector.x;

        bool isNeedToFlip = mouseX <= playerX;

        if (isNeedToFlip)
        {
            Debug.Log("Need flip sword!");
        } else
        {
            Debug.Log("Need return sword into entry position!");
        }
    }
}
