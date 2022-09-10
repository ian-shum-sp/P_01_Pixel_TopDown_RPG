using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    #region class members
    private Equipment _unequippedEquipment;
    private string _unequippedEquipmentInventorySlotID;
    private Equipment _equippedEquipment;
    private string _equippedEquipmentInventorySlotID;
    public Image playerSprite;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI lastSavedText;
    public TextMeshProUGUI healthPointsText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI equipmentWeaponStatInfoText;
    public TextMeshProUGUI equipmentArmorStatInfoText;
    public TextMeshProUGUI experienceRequireText;
    public TextMeshProUGUI experienceText;
    public Image experienceBarMask;
    //Equipped equipments (exclude pouch)
    public DisplaySlot[] displaySlots;
    public TextMeshProUGUI armorInventoryUpgradeButtonText;
    public TextMeshProUGUI weaponInventoryUpgradeButtonText;
    public TextMeshProUGUI potionInventoryUpgradeButtonText;
    public TextMeshProUGUI pouchInventoryUpgradeButtonText;
    public Image unequippedEquipmentSprite;
    public Image unequippedWeaponSprite;
    public Image unequippedPotionSprite;
    public TextMeshProUGUI unequippedEquipmentText;
    public TextMeshProUGUI unequippedEquipmentInfoText;
    public Button equipButton;
    public Image equippedEquipmentSprite;
    public Image equippedWeaponSprite;
    public Image equippedPotionSprite;
    public TextMeshProUGUI equippedEquipmentText;
    public TextMeshProUGUI equippedEquipmentInfoText;
    public Button unequipButton;
    public Animator animator;
    #endregion

    #region accessors

    #endregion
    
    private void UpdateUpgradeButtonText(TextMeshProUGUI text, string upgradeText, Color color)
    {
            text.text = upgradeText;
            text.color = color;
    }

    public void InitializePlayerMenu()
    {
        playerSprite.sprite = GameManager.Instance.playerSprites[(int)GameManager.Instance.player.Gender];
        playerNameText.text = "Name: " + GameManager.Instance.player.Name;
        genderText.text = "Gender: " + Common.GetEnumDescription(GameManager.Instance.player.Gender);
        ResetEquipmentInfoPanels();
    }

    public void UpdateOnExpandPlayerMenu()
    {
        ResetEquipmentInfoPanels();
        UpdateLastSaveTime();
        UpdateHealthPoints();
        UpdateGold();
        UpdateExperience();
        UpdateInventoryUpgradeStatus();
        UpdateDisplaySlot();
        UpdateArmorStatInfoText();
        UpdateWeaponStatInfoText();
    }

    public void UpdateLastSaveTime()
    {
        lastSavedText.text = "Last Saved On: " + GameManager.Instance.LastSavedTimeText;
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void UpdateHealthPoints()
    {
        healthPointsText.text = "Health Points: " + Mathf.CeilToInt(GameManager.Instance.player.healthPoints).ToString() + "/" + Mathf.CeilToInt(GameManager.Instance.player.maxHealthPoints).ToString();
    }

    public void UpdateGold()
    {
        goldText.text = "Gold: " + GameManager.Instance.player.Gold.ToString();
    }

    public void UpdateExperience()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        if(playerLevel == GameManager.Instance.experienceManager.experienceTable.Count)
        {
            levelText.text = "Level: 25 (MAX LEVEL)";
            experienceRequireText.text = "Experience Points to next level: -";
            experienceText.text = "Total Experience Points: " + GameManager.Instance.player.Experience.ToString() + " Experience";
            experienceBarMask.fillAmount = 1.0f;
        }
        else
        {
            levelText.text = "Level: " + playerLevel.ToString();
            int previousLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel-1);
            int currentLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel);
            int experienceNeededToReachNextLevel = currentLevelAccumulatedExperience - previousLevelAccumulatedExperience;
            int currentPlayerExperienceIntoLevel = GameManager.Instance.player.Experience - previousLevelAccumulatedExperience;
            float experienceRatio = (float)currentPlayerExperienceIntoLevel / (float)experienceNeededToReachNextLevel;
            int differenceInExperienceToNextLevel = experienceNeededToReachNextLevel - currentPlayerExperienceIntoLevel;
            experienceRequireText.text = "Experience Points to next level: " + differenceInExperienceToNextLevel.ToString() + " Experience";
            experienceText.text = currentPlayerExperienceIntoLevel.ToString() + "/" + experienceNeededToReachNextLevel.ToString();
            experienceBarMask.fillAmount = experienceRatio;
}
    }

    public void UpdateInventoryUpgradeStatus()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
        if(armorInventory.InventoryLevel == armorInventory.maxLevel)
        {
            UpdateUpgradeButtonText(armorInventoryUpgradeButtonText, "FULLY UPGRADED", Color.black);
        }   
        else
        {
            if(playerLevel >= armorInventory.upgradeLevelRequirements[armorInventory.InventoryLevel])
            {
                string upgradeText = armorInventory.upgradePrices[armorInventory.InventoryLevel].ToString() + " Gold";
                Color textColor = GameManager.Instance.player.Gold >= armorInventory.upgradePrices[armorInventory.InventoryLevel] ? Color.black : Color.red;
                UpdateUpgradeButtonText(armorInventoryUpgradeButtonText, upgradeText, textColor);
            }
            else
            {
                string upgradeText = "Require Level " + armorInventory.upgradeLevelRequirements[armorInventory.InventoryLevel].ToString();
                UpdateUpgradeButtonText(armorInventoryUpgradeButtonText, upgradeText, Color.red);
            }
        }

        Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
        if(weaponInventory.InventoryLevel == weaponInventory.maxLevel)
        {
            UpdateUpgradeButtonText(weaponInventoryUpgradeButtonText, "FULLY UPGRADED", Color.black);
        }  
        else
        {
            if(playerLevel >= weaponInventory.upgradeLevelRequirements[weaponInventory.InventoryLevel])
            {
                string upgradeText = weaponInventory.upgradePrices[weaponInventory.InventoryLevel].ToString() + " Gold";
                Color textColor = GameManager.Instance.player.Gold >= weaponInventory.upgradePrices[weaponInventory.InventoryLevel] ? Color.black : Color.red;
                UpdateUpgradeButtonText(weaponInventoryUpgradeButtonText, upgradeText, textColor);
            }
            else
            {
                string upgradeText = "Require Level " + weaponInventory.upgradeLevelRequirements[weaponInventory.InventoryLevel].ToString();
                UpdateUpgradeButtonText(weaponInventoryUpgradeButtonText, upgradeText, Color.red);
            }
        }

        Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
        if(potionInventory.InventoryLevel == potionInventory.maxLevel)
        {
            UpdateUpgradeButtonText(potionInventoryUpgradeButtonText, "FULLY UPGRADED", Color.black);
        } 
        else
        {
            if(playerLevel >= potionInventory.upgradeLevelRequirements[potionInventory.InventoryLevel])
            {
                string upgradeText = potionInventory.upgradePrices[potionInventory.InventoryLevel].ToString() + " Gold";
                Color textColor = GameManager.Instance.player.Gold >= potionInventory.upgradePrices[potionInventory.InventoryLevel] ? Color.black : Color.red;
                UpdateUpgradeButtonText(potionInventoryUpgradeButtonText, upgradeText, textColor);
            }
            else
            {
                string upgradeText = "Require Level " + potionInventory.upgradeLevelRequirements[potionInventory.InventoryLevel].ToString();
                UpdateUpgradeButtonText(potionInventoryUpgradeButtonText, upgradeText, Color.red);
            }
        }

        Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
        if(pouchInventory.InventoryLevel == pouchInventory.maxLevel)
        {
            UpdateUpgradeButtonText(pouchInventoryUpgradeButtonText, "FULLY UPGRADED", Color.black);
        } 
        else
        {
            if(playerLevel >= pouchInventory.upgradeLevelRequirements[pouchInventory.InventoryLevel])
            {
                string upgradeText = pouchInventory.upgradePrices[pouchInventory.InventoryLevel].ToString() + " Gold";
                Color textColor = GameManager.Instance.player.Gold >= pouchInventory.upgradePrices[pouchInventory.InventoryLevel] ? Color.black : Color.red;
                UpdateUpgradeButtonText(pouchInventoryUpgradeButtonText, upgradeText, textColor);
            }
            else
            {
                string upgradeText = "Require Level " + pouchInventory.upgradeLevelRequirements[pouchInventory.InventoryLevel].ToString();
                UpdateUpgradeButtonText(pouchInventoryUpgradeButtonText, upgradeText, Color.red);
            }
        }
    }

    public void UpdateArmorStatInfoText()
    {
        //Order: Movement Speed, Armor, Damage Reduction, Bleeding Resistance, Knockback Resistance, Element Resistance
        Protection protection = GameManager.Instance.player.GetArmorInfo();

        equipmentArmorStatInfoText.text = "\n" + 
                                        protection.movementSpeed.ToString() + "\n" +
                                        protection.armorPoints.ToString() + "\n" + 
                                        protection.armorPoints.ToString() + "%\n" + 
                                        (protection.GetTotalBleedingResistanceLevel() < 0 ? "0" : protection.GetTotalBleedingResistanceLevel().ToString()) + "\n" +
                                        (protection.GetTotalKnockbackResistanceLevel() < 0 ? "0" : protection.GetTotalKnockbackResistanceLevel().ToString()) + "\n" +
                                        (protection.GetTotalElementResistanceLevel() < 0 ? "0" : protection.GetTotalElementResistanceLevel().ToString());
    }

    public void UpdateWeaponStatInfoText()
    {
        //Order: Weapon Type, Damage Points, Attack Range, Attack Speed, Bleeding, Knockback, Element
        Damage damage = GameManager.Instance.player.GetWeaponInfo();

        equipmentWeaponStatInfoText.text = (damage.isHaveWeapon ? (damage.isMelee ? "Melee" : "Range") : "-") + "\n" + 
                                            (damage.isHaveWeapon ? Mathf.CeilToInt(damage.damagePoints).ToString() : "0") + "\n" + 
                                            damage.attackRange.ToString() + "\n" +
                                            damage.attackSpeed.ToString() + "\n" +
                                            damage.weaponDebuffsLevels[0].ToString() + "\n" +
                                            damage.weaponDebuffsLevels[1].ToString() + "\n" +
                                            damage.weaponDebuffsLevels[2].ToString();
    }

    public void AddToDisplaySlot(Equipment equipment)
    {
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.HEAD_ARMOR].AddToSlot(equipment);
                UpdateArmorStatInfoText();
                break;
            }
            case Common.EquipmentType.CHEST_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.CHEST_ARMOR].AddToSlot(equipment);
                UpdateArmorStatInfoText();
                break;
            }
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.BOOTS_ARMOR].AddToSlot(equipment);
                UpdateArmorStatInfoText();
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                displaySlots[(int)Common.DisplaySlotType.WEAPON].AddToSlot(equipment);
                UpdateWeaponStatInfoText();
                break;
            }
            default:
                break;
        }
    }

    public void RemoveFromDisplaySlot(Equipment equipment)
    {
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.HEAD_ARMOR].RemoveFromSlot();
                UpdateArmorStatInfoText();
                break;
            }
            case Common.EquipmentType.CHEST_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.CHEST_ARMOR].RemoveFromSlot();
                UpdateArmorStatInfoText();
                break;
            }
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.BOOTS_ARMOR].RemoveFromSlot();
                UpdateArmorStatInfoText();
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                displaySlots[(int)Common.DisplaySlotType.WEAPON].RemoveFromSlot();
                UpdateWeaponStatInfoText();
                break;
            }
            default:
                break;
        }
    }

    public void UpdateDisplaySlot()
    {
        for (int i = 0; i < displaySlots.Length; i++)
        {
            displaySlots[i].UpdateDisplaySlot();
        }
    }

    public void ResetEquipmentInfoPanels()
    {
        ResetEquippedEquipmentInfo();
        ResetUnequippedEquipmentInfo();
        Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
        Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
        Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
        armorInventory.DeselectAnySelectedSlots();
        weaponInventory.DeselectAnySelectedSlots();
        potionInventory.DeselectAnySelectedSlots();
    }


    public void ResetUnequippedEquipmentInfo()
    {
        _unequippedEquipment = null;
        _unequippedEquipmentInventorySlotID = null;
        unequippedEquipmentSprite.sprite = null;
        unequippedEquipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        unequippedWeaponSprite.sprite = null;
        unequippedWeaponSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        unequippedPotionSprite.sprite = null;
        unequippedPotionSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        unequippedEquipmentText.text = "NOT SELECTED";
        unequippedEquipmentInfoText.text = "Select an unequipped equipment to display its info";
        unequippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Middle;
        equipButton.gameObject.SetActive(false);
    }

    public void ResetEquippedEquipmentInfo()
    {
        _equippedEquipment = null;
        _equippedEquipmentInventorySlotID = null;
        equippedEquipmentSprite.sprite = null;
        equippedEquipmentSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equippedWeaponSprite.sprite = null;
        equippedWeaponSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equippedPotionSprite.sprite = null;
        equippedPotionSprite.color = Common.UnoccupiedSlotImageBackgroundColor;
        equippedEquipmentText.text = "NOT SELECTED";
        equippedEquipmentInfoText.text = "Select an equipped equipment to display its info";
        equippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Middle;
        unequipButton.gameObject.SetActive(false);
    }

    public void SetUnequippedEquipmentInfo(Equipment equipment, int amount, string inventorySlotID)
    {
        ResetUnequippedEquipmentInfo();
        _unequippedEquipment = equipment;
        _unequippedEquipmentInventorySlotID = inventorySlotID;
        equipButton.gameObject.SetActive(true);
        switch(_unequippedEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                Armor armor = _unequippedEquipment as Armor;
                unequippedEquipmentSprite.sprite = armor.equipmentSprite;
                unequippedEquipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
                unequippedEquipmentText.text = armor.equipmentName;
                unequippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                unequippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(armor.equipmentType) + "\n" +
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
                Weapon weapon = _unequippedEquipment as Weapon;
                unequippedWeaponSprite.sprite = weapon.equipmentSprite;
                unequippedWeaponSprite.color = Common.OccupiedSlotImageBackgroundColor;
                unequippedEquipmentText.text = weapon.equipmentName;
                unequippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                unequippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(weapon.equipmentType) + "\n" +
                                            "Damage Points: " + weapon.damagePoints.ToString() + "\n" +
                                            "Attack Range: " + weapon.attackRange.ToString() + "\n" +
                                            "Attack Speed: " + Mathf.FloorToInt((2.5f - weapon.cooldown)*20).ToString()+ "\n" +
                                            "Level Requirement: " + weapon.levelRequirement.ToString() + "\n" +
                                            "Purchase Price: " + weapon.purchasePrice.ToString() + "\n" +
                                            "Sell Price: " + Mathf.FloorToInt(weapon.purchasePrice/2.0f).ToString() + "\n" +
                                            "Weapon Buffs: " + Common.GetEnumDescription(weapon.weaponDebuff) + (weapon.weaponDebuff == Common.Debuff.NONE ? "" : " Level " + weapon.debuffLevel);
                break;
            }
            case Common.EquipmentType.POTION:
            {
                Potion potion = _unequippedEquipment as Potion;
                equippedPotionSprite.sprite = potion.equipmentSprite;
                equippedPotionSprite.color = Common.OccupiedSlotImageBackgroundColor;
                unequippedEquipmentText.text = potion.equipmentName;
                unequippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                unequippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(potion.equipmentType) + "\n" +
                                            "Duration: " + potion.duration.ToString() + " seconds\n" +
                                            "Cooldown: " + potion.cooldown.ToString() + " seconds\n" +
                                            "Amount in inventory: " + amount.ToString() + "\n" +
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

    public void SetEquippedEquipmentInfo(bool isAutomatic, Equipment equipment = null, int amount = 0, string inventorySlotID = null)
    {
        ResetEquippedEquipmentInfo();
        if(isAutomatic)
        {
            switch(_unequippedEquipment.equipmentType)
            {
                case Common.EquipmentType.HEAD_ARMOR:
                case Common.EquipmentType.CHEST_ARMOR:
                case Common.EquipmentType.BOOTS_ARMOR:
                {
                    Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                    InventorySlot equippedSlot = armorInventory.slots.FirstOrDefault(x => x.IsOccupied && x.Equipment.equipmentType == _unequippedEquipment.equipmentType && x.IsEquipped);
                    if(equippedSlot != null)
                    {
                        unequipButton.gameObject.SetActive(true);
                        equippedSlot.SelectSlot();
                        _equippedEquipment = equippedSlot.Equipment;
                        _equippedEquipmentInventorySlotID = equippedSlot.inventorySlotID;
                        Armor armor = _equippedEquipment as Armor;
                        equippedEquipmentSprite.sprite = armor.equipmentSprite;
                        equippedEquipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
                        equippedEquipmentText.text = armor.equipmentName;
                        equippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                        equippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(armor.equipmentType) + "\n" +
                                                    "Armor Points: " + armor.armorPoints.ToString() + "\n" +
                                                    "Level Requirement: " + armor.levelRequirement.ToString() + "\n" +
                                                    "Purchase Price: " + armor.purchasePrice.ToString() + "\n" +
                                                    "Sell Price: " + Mathf.FloorToInt(armor.purchasePrice/2.0f).ToString() + "\n" +
                                                    "Armor Buffs: " + Common.GetEnumDescription(armor.armorBuff) + (armor.armorBuff == Common.ArmorBuff.NONE ? "" : " Level " + armor.buffLevel);
                    }
                    break;
                }
                case Common.EquipmentType.MELEE_WEAPON:
                case Common.EquipmentType.RANGED_WEAPON:
                {
                    Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
                    InventorySlot equippedSlot = weaponInventory.slots.FirstOrDefault(x => x.IsOccupied && x.IsEquipped);
                    if(equippedSlot != null)
                    {
                        unequipButton.gameObject.SetActive(true);
                        equippedSlot.SelectSlot();
                        _equippedEquipment = equippedSlot.Equipment;
                        _equippedEquipmentInventorySlotID = equippedSlot.inventorySlotID;
                        Weapon weapon = _equippedEquipment as Weapon;
                        equippedWeaponSprite.sprite = weapon.equipmentSprite;
                        equippedWeaponSprite.color = Common.OccupiedSlotImageBackgroundColor;
                        equippedEquipmentText.text = weapon.equipmentName;
                        equippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                        equippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(weapon.equipmentType) + "\n" +
                                                    "Damage Points: " + weapon.damagePoints.ToString() + "\n" +
                                                    "Attack Range: " + weapon.attackRange.ToString() + "\n" +
                                                    "Attack Speed: " + Mathf.FloorToInt((2.5f - weapon.cooldown)*20).ToString()+ "\n" +
                                                    "Level Requirement: " + weapon.levelRequirement.ToString() + "\n" +
                                                    "Purchase Price: " + weapon.purchasePrice.ToString() + "\n" +
                                                    "Sell Price: " + Mathf.FloorToInt(weapon.purchasePrice/2.0f).ToString() + "\n" +
                                                    "Weapon Buffs: " + Common.GetEnumDescription(weapon.weaponDebuff) + (weapon.weaponDebuff == Common.Debuff.NONE ? "" : " Level " + weapon.debuffLevel);
                    }
                    break;
                }
                default:
                    break;
            }
        }
        else
        {
            _equippedEquipment = equipment;
            _equippedEquipmentInventorySlotID = inventorySlotID;
            unequipButton.gameObject.SetActive(true);
            switch(_equippedEquipment.equipmentType)
            {
                case Common.EquipmentType.HEAD_ARMOR:
                case Common.EquipmentType.CHEST_ARMOR:
                case Common.EquipmentType.BOOTS_ARMOR:
                {
                    Armor armor = _equippedEquipment as Armor;
                    equippedEquipmentSprite.sprite = armor.equipmentSprite;
                    equippedEquipmentSprite.color = Common.OccupiedSlotImageBackgroundColor;
                    equippedEquipmentText.text = armor.equipmentName;
                    equippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                    equippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(armor.equipmentType) + "\n" +
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
                    Weapon weapon = _equippedEquipment as Weapon;
                    equippedWeaponSprite.sprite = weapon.equipmentSprite;
                    equippedWeaponSprite.color = Common.OccupiedSlotImageBackgroundColor;
                    equippedEquipmentText.text = weapon.equipmentName;
                    equippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                    equippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(weapon.equipmentType) + "\n" +
                                                "Damage Points: " + weapon.damagePoints.ToString() + "\n" +
                                                "Attack Range: " + weapon.attackRange.ToString() + "\n" +
                                                "Attack Speed: " + Mathf.FloorToInt((2.5f - weapon.cooldown)*20).ToString()+ "\n" +
                                                "Level Requirement: " + weapon.levelRequirement.ToString() + "\n" +
                                                "Purchase Price: " + weapon.purchasePrice.ToString() + "\n" +
                                                "Sell Price: " + Mathf.FloorToInt(weapon.purchasePrice/2.0f).ToString() + "\n" +
                                                "Weapon Buffs: " + Common.GetEnumDescription(weapon.weaponDebuff) + (weapon.weaponDebuff == Common.Debuff.NONE ? "" : " Level " + weapon.debuffLevel);
                    break;
                }
                case Common.EquipmentType.POTION:
                {
                    Potion potion = _equippedEquipment as Potion;
                    equippedPotionSprite.sprite = potion.equipmentSprite;
                    equippedPotionSprite.color = Common.OccupiedSlotImageBackgroundColor;
                    equippedEquipmentText.text = potion.equipmentName;
                    equippedEquipmentInfoText.verticalAlignment = VerticalAlignmentOptions.Top;
                    equippedEquipmentInfoText.text = "Type: " + Common.GetEnumDescription(potion.equipmentType) + "\n" +
                                                "Duration: " + potion.duration.ToString() + " seconds\n" +
                                                "Cooldown: " + potion.cooldown.ToString() + " seconds\n" +
                                                "Amount in inventory: " + amount.ToString() + "\n" +
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
    }

    public void Equip()
    {
        switch(_unequippedEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                armorInventory.slots.First(x => x.inventorySlotID == _unequippedEquipmentInventorySlotID).TryEquip();
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
                weaponInventory.slots.First(x => x.inventorySlotID == _unequippedEquipmentInventorySlotID).TryEquip();
                break;
            }
            case Common.EquipmentType.POTION:
            {
                if(GameManager.Instance.CheckIsAtCentralHub() == true)
                {
                    Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
                    potionInventory.slots.First(x => x.inventorySlotID == _unequippedEquipmentInventorySlotID).TryEquip();
                }
                else
                {
                    GameManager.Instance.ShowNotification("You can only equip/unequip potion in central hubs!", Color.red);
                }
                break;
            }
            default:
                break;
        }
    }

    public void Unequip()
    {
        switch(_equippedEquipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            case Common.EquipmentType.CHEST_ARMOR:
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                GameManager.Instance.player.UnequipArmor(_equippedEquipment as Armor);
                Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
                armorInventory.slots.First(x => x.inventorySlotID == _equippedEquipmentInventorySlotID).UnequipEquipments();
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                GameManager.Instance.player.UnequipWeapon(_equippedEquipment.equipmentID);
                Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
                weaponInventory.slots.First(x => x.inventorySlotID == _equippedEquipmentInventorySlotID).UnequipEquipments();
                break;
            }
            case Common.EquipmentType.POTION:
            {
                if(GameManager.Instance.CheckIsAtCentralHub() == true)
                {
                    Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
                    potionInventory.slots.First(x => x.IsOccupied && x.Equipment.equipmentID == _equippedEquipment.equipmentID).UnequipPotions();
                    potionInventory.slots.First(x => x.inventorySlotID == _equippedEquipmentInventorySlotID).UnequipEquipments();
                }
                else
                {
                    GameManager.Instance.ShowNotification("You can only equip/unequip potion in central hubs!", Color.red);
                }
                break;
            }
            default:
                break;
        }
    }
}
