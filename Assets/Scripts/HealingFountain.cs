using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable
{
    private float _healCooldown = 1.0f;
    private float _lastHealTime;
    public int healingAmount = 1;

    protected override void OnCollide(Collider2D collider)
    {
        if(collider.name != "Player")
            return;

        if(Time.time - _lastHealTime > _healCooldown)
        {
            _lastHealTime = Time.time;
            GameManager.Instance.player.Heal(healingAmount);
        }
    }
}
