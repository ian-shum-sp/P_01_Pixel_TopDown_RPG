using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerResetter : Collidable
{
    private bool isReset = false;
    protected override void OnCollide(Collider2D collider)
    {
        if(isReset)
            return;

        if(collider.name == "Player")
        {
            isReset = true;
            GameManager.Instance.player.healthPoints = 100.0f;
            GameManager.Instance.player.maxHealthPoints = 100.0f;
            GameManager.Instance.player.Gold = 0;
            GameManager.Instance.player.Experience = 0;
            GameManager.Instance.player.UnequipAllEquipment();
            GameManager.Instance.player.InitializeInventory(Common.InventoryType.ARMOR, 1);
            GameManager.Instance.player.InitializeInventory(Common.InventoryType.WEAPON, 1);
            GameManager.Instance.player.InitializeInventory(Common.InventoryType.POTION, 1);
            GameManager.Instance.player.InitializeInventory(Common.InventoryType.POUCH, 1);
        }
    }
}
