using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collidable
{
    private bool _isCollected = false;
    private SpriteRenderer _spriteRender;
    public Sprite emptyChestSprite;
    public Common.ChestType chestType;

    protected override void Start() 
    {
        base.Start();
        _spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Collect()
    {
        _isCollected = true;
        AddEquipmentActionResult result = new AddEquipmentActionResult(); 
        result.isAdded = true;

        switch(chestType)
        {
            case Common.ChestType.STARTER_CHEST:
            {
                List<Equipment> starterEquipments = GameManager.Instance.GetStarterEquipments();
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[0]);
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[1]);
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[2]);
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[3]);
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[4]);
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[5], 1);
                #region For testing only -> uncomment if unneeded
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[6]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[7]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[8]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[9]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[10]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[11]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[12]);
                // GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[13]);
                #endregion
                GameManager.Instance.ShowFloatingText("Acquire starter equipments!", 30, Color.yellow, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 30, 3.0f);
                break;
            }
            case Common.ChestType.GOLD_CHEST_SILVER:
            {
                int randomGoldAmount = Random.Range(20, 61);
                GameManager.Instance.player.Gold += randomGoldAmount;
                GameManager.Instance.ShowFloatingText("+ " + randomGoldAmount + " gold!", 30, Color.yellow, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 25, 2.0f);
                break;
            }
            case Common.ChestType.GOLD_CHEST_GOLDEN:
            {
                int randomGoldAmount = Random.Range(100, 201);
                GameManager.Instance.player.Gold += randomGoldAmount;
                GameManager.Instance.ShowFloatingText("+ " + randomGoldAmount + " gold!", 30, Color.yellow, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 25, 2.0f);
                break;
            }
            case Common.ChestType.ARMOR_CHEST:
            {
                Equipment armor = GameManager.Instance.GenerateRandomArmor();
                result = GameManager.Instance.player.AddEquipmentToInventory(armor, isContinueBlockGameAction: false);
                if(result.isAdded)
                    GameManager.Instance.ShowFloatingText("Acquire " + armor.equipmentName + "!", 30, Color.blue, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 25, 2.0f);
                break;
            }
            case Common.ChestType.WEAPON_CHEST:
            {
                Equipment weapon = GameManager.Instance.GenerateRandomWeapon();
                result = GameManager.Instance.player.AddEquipmentToInventory(weapon, isContinueBlockGameAction: false);
                if(result.isAdded)
                    GameManager.Instance.ShowFloatingText("Acquire " + weapon.equipmentName + "!", 30, Color.blue, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 25, 2.0f);
                break;
            }
            case Common.ChestType.POTION_CHEST:
            {
                Equipment potion = GameManager.Instance.GenerateRandomPotion();
                result = GameManager.Instance.player.AddEquipmentToInventory(potion, 1, false);
                if(result.isAdded)
                    GameManager.Instance.ShowFloatingText("Acquire " + potion.equipmentName + "!", 30, Color.blue, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 25, 2.0f);
                break;
            }
            default:
                break;
        }

        if(result.isAdded)
            _spriteRender.sprite = emptyChestSprite;
    }

    protected override void OnCollide(Collider2D collider)
    {
        if(_isCollected)
            return;

        if(collider.name == "Player")
            Collect();
    }
}
