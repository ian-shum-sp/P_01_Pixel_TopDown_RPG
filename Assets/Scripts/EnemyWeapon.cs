using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : CharacterWeapon
{
    protected override void OnCollide(Collider2D collider)
    {
        if(collider.tag == "Fighter" && collider.name == "Player")
        {
            collider.SendMessage("ReceiveDamage", _currentDamage);
        }
    }
}
