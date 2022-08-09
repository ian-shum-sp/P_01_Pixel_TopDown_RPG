using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private Shop _currentActiveShop;
    private Equipment _selectedBuyEquipment;
    private Equipment _selectedSellEquipment;
    private int _sellEquipmentAmount;
    private string _sellEquipmentInventoryID;
    private bool _isTryBuy;
    private bool _isTrySell;
    private bool _isInitialized = false;
    public SellSlot[] playerArmorSellSlots;
    public SellSlot[] playerWeaponSellSlots;
    public SellSlot[] playerPotionSellSlots;
    public TextMeshProUGUI inventoryTitle;
    public Shop[] shops;
    public Image buyEquipmentSprite;
    public Image buyWeaponSprite;
    public Image buyPotionSprite;
    public TextMeshProUGUI buyEquipmentText;
    public TextMeshProUGUI buyEquipmentInfoText;
    public Image sellEquipmentSprite;
    public Image sellWeaponSprite;
    public Image sellPotionSprite;
    public TextMeshProUGUI sellEquipmentText;
    public TextMeshProUGUI sellEquipmentInfoText;
    public TextMeshProUGUI goldInfoText;
    public Button purchaseButton;
    public Button sellButton;
    public Animator animator;

    private void Update()
    {
        if(!_isTryBuy && !_isTrySell)
            return;

        if(_isTryBuy)
        {
            if(GameManager.Instance.GetConfirmationResult() == true)
            {
                AddEquipmentActionResult result = GameManager.Instance.player.AddEquipmentToInventory(_selectedBuyEquipment);
                if(result.isAdded)
                {
                    GameManager.Instance.player.Gold -= _selectedBuyEquipment.purchasePrice;
                    goldInfoText.text = "Available Gold: " + GameManager.Instance.player.Gold.ToString();
                    AddToSellSlots(_selectedBuyEquipment, result.inventorySlotID);
                }
            }
        }
        
        if(_isTrySell)
        {
            if(GameManager.Instance.GetConfirmationResult() == true)
            {
                GameManager.Instance.player.Gold += Mathf.FloorToInt(_selectedSellEquipment.purchasePrice/2.0f) * _sellEquipmentAmount;
                goldInfoText.text = "Available Gold: " + GameManager.Instance.player.Gold.ToString();
                GameManager.Instance.player.RemoveEquipmentFromInventory(_selectedSellEquipment, _sellEquipmentInventoryID);
                RemoveFromSellSlot();
                DeselectAnyActiveSellSlot();
            }
        }
    }

    private void DeactivateAllShops()
    {
        for (int i = 0; i < shops.Length; i++)
        {
            shops[i].gameObject.SetActive(false);
        }
        _currentActiveShop = null;
        DeselectBuyEquipment();
        DeselectSellEquipment();
        goldInfoText.text = null;
    }

    private void ActivateArmorSellSlots()
    {
        for (int i = 0; i < playerArmorSellSlots.Length; i++)
        {
            playerArmorSellSlots[i].gameObject.SetActive(true);
        }
    }

    private void DeactivateArmorSellSlots()
    {
        for (int i = 0; i < playerArmorSellSlots.Length; i++)
        {
            playerArmorSellSlots[i].gameObject.SetActive(false);
        }
        inventoryTitle.text = null;
    }

    private void ActivateWeaponSellSlots()
    {
        for (int i = 0; i < playerWeaponSellSlots.Length; i++)
        {
            playerWeaponSellSlots[i].gameObject.SetActive(true);
        }
        inventoryTitle.text = null;
    }

    private void DeactivateWeaponSellSlots()
    {
        for (int i = 0; i < playerWeaponSellSlots.Length; i++)
        {
            playerWeaponSellSlots[i].gameObject.SetActive(false);
        }
        inventoryTitle.text = null;
    }

    private void ActivatePotionSellSlots()
    {
        for (int i = 0; i < playerPotionSellSlots.Length; i++)
        {
            playerPotionSellSlots[i].gameObject.SetActive(true);
        }
    }

    private void DeactivatePotionSellSlots()
    {
        for (int i = 0; i < playerPotionSellSlots.Length; i++)
        {
            playerPotionSellSlots[i].gameObject.SetActive(false);
        }
    }

    private void DeselectBuyEquipment()
    {
        _selectedBuyEquipment = null;
        buyEquipmentSprite.sprite = null;
        buyEquipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        buyWeaponSprite.sprite = null;
        buyWeaponSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        buyPotionSprite.sprite = null;
        buyPotionSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        buyEquipmentText.text = "NOT SELECTED";
        buyEquipmentInfoText.text = "Please select an item to display its info";
        purchaseButton.gameObject.SetActive(false);
    }

    private void DeselectSellEquipment()
    {
        _selectedSellEquipment = null;
        _sellEquipmentAmount = 0;
        _sellEquipmentInventoryID = null;
        sellEquipmentSprite.sprite = null;
        sellEquipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        sellWeaponSprite.sprite = null;
        sellWeaponSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        sellPotionSprite.sprite = null;
        sellPotionSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        sellEquipmentText.text = "NOT SELECTED";
        sellEquipmentInfoText.text = "Please select an item to display its info";
        sellButton.gameObject.SetActive(false);
    }

    private void RemoveFromSellSlot()
    {
        switch(_selectedSellEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                playerArmorSellSlots.First(x => x.InventorySlotID == _sellEquipmentInventoryID).RemoveFromSlot();
                break;
            }  
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                playerWeaponSellSlots.First(x => x.InventorySlotID == _sellEquipmentInventoryID).RemoveFromSlot();
                break;
            }   
            case Common.EquipmentType.POTION:
            {
                playerPotionSellSlots.First(x => x.InventorySlotID == _sellEquipmentInventoryID).RemoveFromSlot();
                break;
            } 
            default: 
                break;
        }
    }

    public void InitializeShops()
    {
        if(_isInitialized)
            return;

        UpdateSellSlots();
        DeactivateAllShops();
        DeactivateArmorSellSlots();
        DeactivateWeaponSellSlots();
        DeactivatePotionSellSlots();
        _isInitialized = true;
    }

    public void ShowShop(Common.NPCType shopOwner)
    {
        animator.SetTrigger("Show");
        goldInfoText.text = "Available Gold: " + GameManager.Instance.player.Gold.ToString();
        _currentActiveShop = shops.First(x => x.shopOwner == shopOwner);
        _currentActiveShop.gameObject.SetActive(true);

        switch(_currentActiveShop.shopOwner)
        {
            case Common.NPCType.DUNGEON_ARMORER:
            case Common.NPCType.ENCHANTED_FOREST_ARMORER:
            case Common.NPCType.FANTASY_ARMORER:
            {
                ActivateArmorSellSlots();
                inventoryTitle.text = "Armor Inventory";
                break;
            }
            case Common.NPCType.DUNGEON_WEAPONSMITH:
            case Common.NPCType.ENCHANTED_FOREST_WEAPONSMITH:
            case Common.NPCType.FANTASY_WEAPONSMITH:
            {
                ActivateWeaponSellSlots();
                inventoryTitle.text = "Weapon Inventory";
                break;
            }
            case Common.NPCType.DUNGEON_POTION_BREWER:
            case Common.NPCType.ENCHANTED_FOREST_POTION_BREWER:
            case Common.NPCType.FANTASY_POTION_BREWER:
            {
                ActivatePotionSellSlots();
                inventoryTitle.text = "Potion Inventory";
                break;
            }
        }
    }

    public void HideShop()
    {
        animator.SetTrigger("Hide");
        switch(_currentActiveShop.shopOwner)
        {
            case Common.NPCType.DUNGEON_ARMORER:
            case Common.NPCType.ENCHANTED_FOREST_ARMORER:
            case Common.NPCType.FANTASY_ARMORER:
            {
                DeactivateArmorSellSlots();
                break;
            }
            case Common.NPCType.DUNGEON_WEAPONSMITH:
            case Common.NPCType.ENCHANTED_FOREST_WEAPONSMITH:
            case Common.NPCType.FANTASY_WEAPONSMITH:
            {
                DeactivateWeaponSellSlots();
                break;
            }
            case Common.NPCType.DUNGEON_POTION_BREWER:
            case Common.NPCType.ENCHANTED_FOREST_POTION_BREWER:
            case Common.NPCType.FANTASY_POTION_BREWER:
            {
                DeactivatePotionSellSlots();
                break;
            }
        }
        DeactivateAllShops();
    }

    public void SetSelectedBuyEquipment(Equipment equipment)
    {
        DeselectBuyEquipment();
        _selectedBuyEquipment = equipment;
        purchaseButton.gameObject.SetActive(true);
        switch(_selectedBuyEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                Armor armor = _selectedBuyEquipment as Armor;
                buyEquipmentSprite.sprite = armor.equipmentSprite;;
                buyEquipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
                buyEquipmentText.text = armor.equipmentName;
                buyEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(armor.equipmentType) + "\n" +
                                            "Armor Points: " + armor.armorPoints.ToString() + "\n" +
                                            "Level Requirement: " + armor.levelRequirement.ToString() + "\n" +
                                            "Purchase Price: " + armor.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(armor.purchasePrice/2.0f).ToString() + "\n" +
                                            "Armor Buffs: " + Common.GetEnumDescription(armor.armorBuff) + (armor.armorBuff == Common.ArmorBuff.NONE ? "" : " Level " + armor.buffLevel);
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                Weapon weapon = _selectedBuyEquipment as Weapon;
                buyWeaponSprite.sprite = weapon.equipmentSprite;
                buyWeaponSprite.color = Common.OccupiedSlotImageBackgroundColor;
                buyEquipmentText.text = weapon.equipmentName;
                buyEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(weapon.equipmentType) + "\n" +
                                            "Damage Points: " + weapon.damagePoints.ToString() + "\n" +
                                            "Attack Range: " + weapon.attackRange.ToString() + "\n" +
                                            "Attack Speed: " + Mathf.FloorToInt((2.0f - weapon.cooldown)*20).ToString()+ "\n" +
                                            "Level Requirement: " + weapon.levelRequirement.ToString() + "\n" +
                                            "Purchase Price: " + weapon.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(weapon.purchasePrice/2.0f).ToString() + "\n" +
                                            "Weapon Buffs: " + Common.GetEnumDescription(weapon.weaponDebuff) + (weapon.weaponDebuff == Common.Debuff.NONE ? "" : " Level " + weapon.debuffLevel);
                break;
            }
            case Common.EquipmentType.POTION:
            {
                Potion potion = _selectedBuyEquipment as Potion;
                buyPotionSprite.sprite = potion.equipmentSprite;
                buyPotionSprite.color = Common.OccupiedSlotImageBackgroundColor;
                buyEquipmentText.text = potion.equipmentName;
                buyEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(potion.equipmentType) + "\n" +
                                            "Duration: " + potion.duration.ToString() + " seconds\n" +
                                            "Cooldown: " + potion.cooldown.ToString() + "seconds\n" +
                                            "Max number in pouch: " + potion.maxNumberInPouch.ToString() + "\n" +
                                            "Purchase Price: " + potion.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(potion.purchasePrice/2.0f).ToString() + "\n" +
                                            "Effect: " + Common.GetEnumDescription(potion.potionBuff) + " Level " + potion.buffLevel;
                break;
            }
            default:
                break;
        }
    }

    public void SetSelectedSellEquipment(Equipment equipment, int amount, string inventorySlotID)
    {
        DeselectSellEquipment();
        _selectedSellEquipment = equipment;
        _sellEquipmentAmount = amount;
        _sellEquipmentInventoryID = inventorySlotID;
        sellButton.gameObject.SetActive(true);
        switch(_selectedSellEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                Armor armor = _selectedSellEquipment as Armor;
                sellEquipmentSprite.sprite = armor.equipmentSprite;;
                sellEquipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
                sellEquipmentText.text = armor.equipmentName;
                sellEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(armor.equipmentType) + "\n" +
                                            "Armor Points: " + armor.armorPoints.ToString() + "\n" +
                                            "Level Requirement: " + armor.levelRequirement.ToString() + "\n" +
                                            "Purchase Price: " + armor.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(armor.purchasePrice/2.0f).ToString() + "\n" +
                                            "Armor Buffs: " + Common.GetEnumDescription(armor.armorBuff) + (armor.armorBuff == Common.ArmorBuff.NONE ? "" : " Level " + armor.buffLevel);
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                Weapon weapon = _selectedSellEquipment as Weapon;
                sellWeaponSprite.sprite = weapon.equipmentSprite;
                sellWeaponSprite.color = Common.OccupiedSlotImageBackgroundColor;
                sellEquipmentText.text = weapon.equipmentName;
                sellEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(weapon.equipmentType) + "\n" +
                                            "Damage Points: " + weapon.damagePoints.ToString() + "\n" +
                                            "Attack Range: " + weapon.attackRange.ToString() + "\n" +
                                            "Attack Speed: " + Mathf.FloorToInt((2.0f - weapon.cooldown)*20).ToString()+ "\n" +
                                            "Level Requirement: " + weapon.levelRequirement.ToString() + "\n" +
                                            "Purchase Price: " + weapon.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(weapon.purchasePrice/2.0f).ToString() + "\n" +
                                            "Weapon Buffs: " + Common.GetEnumDescription(weapon.weaponDebuff) + (weapon.weaponDebuff == Common.Debuff.NONE ? "" : " Level " + weapon.debuffLevel);
                break;
            }
            case Common.EquipmentType.POTION:
            {
                Potion potion = _selectedSellEquipment as Potion;
                sellPotionSprite.sprite = potion.equipmentSprite;
                sellPotionSprite.color = Common.OccupiedSlotImageBackgroundColor;
                sellEquipmentText.text = potion.equipmentName;
                sellEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(potion.equipmentType) + "\n" +
                                            "Duration: " + potion.duration.ToString() + " seconds\n" +
                                            "Cooldown: " + potion.cooldown.ToString() + "seconds\n" +
                                            "Amount in inventory: " + _sellEquipmentAmount.ToString() + "\n" +
                                            "Max number in pouch: " + potion.maxNumberInPouch.ToString() + "\n" +
                                            "Purchase Price: " + potion.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(potion.purchasePrice/2.0f).ToString() + "\n" +
                                            "Effect: " + Common.GetEnumDescription(potion.potionBuff) + " Level " + potion.buffLevel;
                break;
            }
            default:
                break;
        }
    }

    public void DeselectAnyActiveBuySlot()
    {
        ShopSlot activeBuySlot = _currentActiveShop.shopBuySlots.FirstOrDefault(x => x.IsSelected);
        if(activeBuySlot != null)
            activeBuySlot.DeselectSlot();
    }

    public void DeselectAnyActiveSellSlot()
    {
        switch(_currentActiveShop.shopOwner)
        {
            case Common.NPCType.DUNGEON_ARMORER:
            case Common.NPCType.ENCHANTED_FOREST_ARMORER:
            case Common.NPCType.FANTASY_ARMORER:
            {
                ShopSlot activeSellSlot = playerArmorSellSlots.FirstOrDefault(x => x.IsSelected);
                if(activeSellSlot != null)
                    activeSellSlot.DeselectSlot();
                break;
            }
            case Common.NPCType.DUNGEON_WEAPONSMITH:
            case Common.NPCType.ENCHANTED_FOREST_WEAPONSMITH:
            case Common.NPCType.FANTASY_WEAPONSMITH:
            {
                ShopSlot activeSellSlot = playerWeaponSellSlots.FirstOrDefault(x => x.IsSelected);
                if(activeSellSlot != null)
                    activeSellSlot.DeselectSlot();
                break;
            }
            case Common.NPCType.DUNGEON_POTION_BREWER:
            case Common.NPCType.ENCHANTED_FOREST_POTION_BREWER:
            case Common.NPCType.FANTASY_POTION_BREWER:
            {
                ShopSlot activeSellSlot = playerPotionSellSlots.FirstOrDefault(x => x.IsSelected);
                if(activeSellSlot != null)
                    activeSellSlot.DeselectSlot();
                break;
            }
        }
        DeselectSellEquipment();
    }

    public void AddToSellSlots(Equipment equipment, string inventorySlotID, int? amount = null)
    {   
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                SellSlot slot = playerArmorSellSlots.First(x => !x.IsOccupied);
                slot.AddEquipmentToSlot(equipment, inventorySlotID, amount);
                break;
            }  
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                SellSlot slot = playerWeaponSellSlots.First(x => !x.IsOccupied);
                slot.AddEquipmentToSlot(equipment, inventorySlotID, amount);
                break;
            }   
            case Common.EquipmentType.POTION:
            {
                SellSlot slot = playerPotionSellSlots.FirstOrDefault(x => x.Equipment != null && x.Equipment.equipmentID == equipment.equipmentID);
                if(slot == null)
                    slot = playerPotionSellSlots.First(x => !x.IsOccupied);

                slot.AddEquipmentToSlot(equipment, inventorySlotID, amount);
                break;
            } 
            default: 
                break;
        }
    }

    
    public void TryBuy()
    {
        if(GameManager.Instance.player.Gold < _selectedBuyEquipment.purchasePrice)
        {
            GameManager.Instance.ShowWarning("Not enough gold!");
            return;
        }

        _isTryBuy = true;
        GameManager.Instance.ShowConfirmation("Purchase this item for " + _selectedBuyEquipment.purchasePrice.ToString() + " gold?");
    }

    public void TrySell()
    {
        Inventory inventory = null;
        Slot slot = null;
        InventorySlot inventorySlot = null;
        switch(_selectedSellEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                slot = inventory.slots.First(x => x.inventorySlotID == _sellEquipmentInventoryID);
                inventorySlot = slot as InventorySlot;
                if(inventorySlot.IsEquipped)
                    GameManager.Instance.ShowWarning("Item is equipped!");
                else
                    _isTrySell = true;
                    GameManager.Instance.ShowConfirmation("Sell this item for " + _selectedSellEquipment.purchasePrice.ToString() + " gold?");
                break;
            }  
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
                slot = inventory.slots.First(x => x.inventorySlotID == _sellEquipmentInventoryID);
                inventorySlot = slot as InventorySlot;
                if(inventorySlot.IsEquipped)
                    GameManager.Instance.ShowWarning("Item is equipped!");
                else
                    _isTrySell = true;
                    GameManager.Instance.ShowConfirmation("Sell this item for " + _selectedSellEquipment.purchasePrice.ToString() + " gold?");
                break;
            }   
            case Common.EquipmentType.POTION:
            {
                inventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
                slot = inventory.slots.First(x => x.inventorySlotID == _sellEquipmentInventoryID);
                inventorySlot = slot as InventorySlot;
                if(inventorySlot.IsEquipped)
                    GameManager.Instance.ShowWarning("Item is equipped!");
                else
                    _isTrySell = true;
                    GameManager.Instance.ShowConfirmation("Sell this item for " + _selectedSellEquipment.purchasePrice.ToString() + " gold?");
                break;
            } 
            default: 
                break;
        }
    }

    public void UpdateSellSlots()
    {
        Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
        for (int i = 0; i < armorInventory.MaxNumberOfInventorySlots; i++)
        {
            if(i < armorInventory.UnlockedInventorySlots)
            {
                Debug.Log(i);
                playerArmorSellSlots[i].UnlockSlot();
                if(armorInventory.slots[i].IsOccupied)
                    playerArmorSellSlots[i].AddEquipmentToSlot(armorInventory.slots[i].Equipment, armorInventory.slots[i].inventorySlotID, armorInventory.slots[i].Amount);
            }
            else
                playerArmorSellSlots[i].LockSlot();
        }

        Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
        for (int i = 0; i < weaponInventory.MaxNumberOfInventorySlots; i++)
        {
            if(i < weaponInventory.UnlockedInventorySlots)
            {
                playerWeaponSellSlots[i].UnlockSlot();
                if(weaponInventory.slots[i].IsOccupied)
                    playerWeaponSellSlots[i].AddEquipmentToSlot(weaponInventory.slots[i].Equipment, weaponInventory.slots[i].inventorySlotID, weaponInventory.slots[i].Amount);
            }
            else
                playerWeaponSellSlots[i].LockSlot();
        }

        Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
        for (int i = 0; i < potionInventory.MaxNumberOfInventorySlots; i++)
        {
            if(i < potionInventory.UnlockedInventorySlots)
            {
                playerPotionSellSlots[i].UnlockSlot();
                if(potionInventory.slots[i].IsOccupied)
                    playerPotionSellSlots[i].AddEquipmentToSlot(potionInventory.slots[i].Equipment, potionInventory.slots[i].inventorySlotID, potionInventory.slots[i].Amount);
            }
            else
                playerPotionSellSlots[i].LockSlot();
        }
    }
}
