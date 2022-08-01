using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public Image playerSprite;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI lastSavedText;
    public TextMeshProUGUI healthPointsText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI equipmentStatInfoText;
    public Image experienceBarMask;
    public TextMeshProUGUI experienceText;
    //Equipped equipments (exclude pouch)
    public DisplaySlot[] displaySlots;
    public TextMeshProUGUI armorInventoryUpgradeButtonText;
    public TextMeshProUGUI weaponInventoryUpgradeButtonText;
    public TextMeshProUGUI potionInventoryUpgradeButtonText;
    public TextMeshProUGUI pouchInventoryUpgradeButtonText;
    public TextMeshProUGUI equipmentInfoText;

    

    private void SetUpgradeButtonText(TextMeshProUGUI text, string upgradeText, Color color)
    {
            text.text = upgradeText;
            text.color = color;
    }
    
    public void InitializePlayerMenu()
    {
        playerSprite.sprite = GameManager.Instance.playerSprites[(int)GameManager.Instance.player.Gender];
        playerNameText.text = "Name: " + GameManager.Instance.player.Name;
        genderText.text = "Gender: " + Common.GetEnumDescription(GameManager.Instance.player.Gender);
    }

    public void UpdateOnExpandPlayerMenu()
    {
        lastSavedText.text = "Last Saved On: " + GameManager.Instance.LastSavedTimeText;
        UpdateHealthPoints();
        UpdateGold();
        UpdateExperience();
        UpdateInventoryUpgradeStatus();
        UpdateDisplaySlot();
    }

    public void UpdateHealthPoints()
    {
        healthPointsText.text = "Health Points: " + GameManager.Instance.player.healthPoints.ToString() + "/" + GameManager.Instance.player.maxHealthPoints.ToString();
    }

    public void UpdateGold()
    {
        goldText.text = "Gold: " + GameManager.Instance.player.Gold.ToString();
    }

    public void UpdateExperience()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        levelText.text = "Level: " + playerLevel.ToString();
        int previousLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel-1);
        int currentLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel);
        int experienceNeededToReachNextLevel = currentLevelAccumulatedExperience - previousLevelAccumulatedExperience;
        int currentPlayerExperienceIntoLevel = GameManager.Instance.player.Experience - previousLevelAccumulatedExperience;
        float experienceRatio = (float)currentPlayerExperienceIntoLevel / (float)experienceNeededToReachNextLevel;
        experienceText.text = currentPlayerExperienceIntoLevel.ToString() + "/" + experienceNeededToReachNextLevel.ToString();
        experienceBarMask.fillAmount = experienceRatio;
    }

    public void UpdateInventoryUpgradeStatus()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        Inventory armorInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.ARMOR);
        if(playerLevel >= armorInventory.upgradeLevelRequirements[armorInventory.InventoryLevel])
        {
            string upgradeText = armorInventory.upgradePrices[armorInventory.InventoryLevel].ToString() + " Gold";
            Color textColor = GameManager.Instance.player.Gold >= armorInventory.upgradePrices[armorInventory.InventoryLevel] ? Color.white : Color.red;
            SetUpgradeButtonText(armorInventoryUpgradeButtonText, upgradeText, textColor);
        }
        else
        {
            string upgradeText = "Require Level " + armorInventory.upgradeLevelRequirements[armorInventory.InventoryLevel].ToString();
            SetUpgradeButtonText(armorInventoryUpgradeButtonText, upgradeText, Color.red);
        }

        Inventory weaponInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.WEAPON);
        if(playerLevel >= weaponInventory.upgradeLevelRequirements[weaponInventory.InventoryLevel])
        {
            string upgradeText = weaponInventory.upgradePrices[weaponInventory.InventoryLevel].ToString() + " Gold";
            Color textColor = GameManager.Instance.player.Gold >= armorInventory.upgradePrices[armorInventory.InventoryLevel] ? Color.white : Color.red;
            SetUpgradeButtonText(weaponInventoryUpgradeButtonText, upgradeText, textColor);
        }
        else
        {
            string upgradeText = "Require Level " + weaponInventory.upgradeLevelRequirements[weaponInventory.InventoryLevel].ToString();
            SetUpgradeButtonText(weaponInventoryUpgradeButtonText, upgradeText, Color.red);
        }

        Inventory potionInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POTION);
        if(playerLevel >= potionInventory.upgradeLevelRequirements[potionInventory.InventoryLevel])
        {
            string upgradeText = potionInventory.upgradePrices[potionInventory.InventoryLevel].ToString() + " Gold";
            Color textColor = GameManager.Instance.player.Gold >= armorInventory.upgradePrices[armorInventory.InventoryLevel] ? Color.white : Color.red;
            SetUpgradeButtonText(potionInventoryUpgradeButtonText, upgradeText, textColor);
        }
        else
        {
            string upgradeText = "Require Level " + potionInventory.upgradeLevelRequirements[potionInventory.InventoryLevel].ToString();
            SetUpgradeButtonText(potionInventoryUpgradeButtonText, upgradeText, Color.red);
        }

        Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
        if(playerLevel >= pouchInventory.upgradeLevelRequirements[pouchInventory.InventoryLevel])
        {
            string upgradeText = pouchInventory.upgradePrices[pouchInventory.InventoryLevel].ToString() + " Gold";
            Color textColor = GameManager.Instance.player.Gold >= armorInventory.upgradePrices[armorInventory.InventoryLevel] ? Color.white : Color.red;
            SetUpgradeButtonText(pouchInventoryUpgradeButtonText, upgradeText, textColor);
        }
        else
        {
            string upgradeText = "Require Level " + pouchInventory.upgradeLevelRequirements[pouchInventory.InventoryLevel].ToString();
            SetUpgradeButtonText(pouchInventoryUpgradeButtonText, upgradeText, Color.red);
        }
    }

    public void AddToDisplaySlot(Equipment equipment)
    {
        switch(equipment.equipmentType)
        {
            case Common.EquipmentType.HEAD_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.HEAD_ARMOR].AddToSlot(equipment);
                break;
            }
            case Common.EquipmentType.CHEST_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.CHEST_ARMOR].AddToSlot(equipment);
                break;
            }
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.BOOTS_ARMOR].AddToSlot(equipment);
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                displaySlots[(int)Common.DisplaySlotType.WEAPON].AddToSlot(equipment);
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
                break;
            }
            case Common.EquipmentType.CHEST_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.CHEST_ARMOR].RemoveFromSlot();
                break;
            }
            case Common.EquipmentType.BOOTS_ARMOR:
            {
                displaySlots[(int)Common.DisplaySlotType.BOOTS_ARMOR].RemoveFromSlot();
                break;
            }
            case Common.EquipmentType.MELEE_WEAPON:
            case Common.EquipmentType.RANGED_WEAPON:
            {
                displaySlots[(int)Common.DisplaySlotType.WEAPON].RemoveFromSlot();
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
}
