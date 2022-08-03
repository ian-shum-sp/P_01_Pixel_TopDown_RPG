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

    protected override void OnCollide(Collider2D collider)
    {
        if(collider.name == "Player")
            Collect();
    }

    private void Collect()
    {
        if(_isCollected)
            return;

        _isCollected = true;
        _spriteRender.sprite = emptyChestSprite;

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
                GameManager.Instance.player.AddEquipmentToInventory(starterEquipments[5], 15);
                GameManager.Instance.ShowFloatingText("Acquire starter equipments!", 30, Color.yellow, transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 30, 3.0f);
                break;
            }
            case Common.ChestType.GOLD_CHEST_SILVER:
            {
                int randomGoldAmount = Random.Range(20, 61);
                GameManager.Instance.player.Gold += randomGoldAmount;
                GameManager.Instance.ShowFloatingText("+ " + randomGoldAmount + " gold!", 30, Color.yellow, transform.position, Vector3.up * 25, 3.0f);
                break;
            }
            case Common.ChestType.GOLD_CHEST_GOLDEN:
            {
                int randomGoldAmount = Random.Range(100, 201);
                GameManager.Instance.player.Gold += randomGoldAmount;
                GameManager.Instance.ShowFloatingText("+ " + randomGoldAmount + " gold!", 30, Color.yellow, transform.position, Vector3.up * 25, 3.0f);
                break;
            }
            case Common.ChestType.ARMOR_CHEST:
            {
                Equipment armor = GameManager.Instance.GenerateRandomArmor();
                GameManager.Instance.player.AddEquipmentToInventory(armor);
                GameManager.Instance.ShowFloatingText("Acquire " + armor.equipmentName + "!", 30, Color.blue, transform.position, Vector3.up * 25, 3.0f);
                break;
            }
            case Common.ChestType.WEAPON_CHEST:
            {
                Equipment weapon = GameManager.Instance.GenerateRandomWeapon();
                GameManager.Instance.player.AddEquipmentToInventory(weapon);
                GameManager.Instance.ShowFloatingText("Acquire " + weapon.equipmentName + "!", 30, Color.red, transform.position, Vector3.up * 25, 3.0f);
                break;
            }
            case Common.ChestType.POTION_CHEST:
            {
                Equipment potion = GameManager.Instance.GenerateRandomPotion();
                GameManager.Instance.player.AddEquipmentToInventory(potion, 1);
                GameManager.Instance.ShowFloatingText("Acquire " + potion.equipmentName + "!", 30, Color.green, transform.position, Vector3.up * 25, 3.0f);
                break;
            }
            default:
                break;
        }

    }



}
