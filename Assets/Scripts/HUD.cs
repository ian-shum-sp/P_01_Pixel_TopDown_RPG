using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private bool _isInitialized;
    private int _currentBagLevel = 0;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI statusInfoText;
    public Image levelProgressBarMask;
    public Image healthBarMask;
    public PouchSlot[] pouchSlots;
    public Animator animator;
    public Image bagSprite;
    public Sprite[] bagSprites;
    public int[] bagChangeLevelRequirements;

    private void Update()
    {
        if(GameManager.Instance.IsBlockGameActions)
            return;

        if(GameManager.Instance.CheckIsAtCentralHub() == true)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha1))
            pouchSlots[0].TryUsePotion();
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
            pouchSlots[1].TryUsePotion();

        if(Input.GetKeyDown(KeyCode.Alpha3))
            pouchSlots[2].TryUsePotion();

        if(Input.GetKeyDown(KeyCode.Alpha4))
            pouchSlots[3].TryUsePotion();
    }
   
    public void InitializeHUD()
    {
        if(_isInitialized)
            return;
    
        playerNameText.text = GameManager.Instance.player.Name;
        statusInfoText.text = null;
        UpdateExperience();
        UpdateHealthPoints();
        Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
        for(int i = 0; i < pouchInventory.maxLevel; i++)
        {
            if(i < pouchInventory.InventoryLevel)
                pouchSlots[i].UnlockSlot();
            else
                pouchSlots[i].LockSlot();
        }
        _isInitialized = true;
    }

    public void ResetHUD()
    {
        for(int i = 0; i < pouchSlots.Length; i++)
        {
            if(pouchSlots[i].IsOccupied)
                pouchSlots[i].RemoveAllEffectsAndTimers();
        }
        _isInitialized = false;
    }

    public void UpdateExperience()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();

        if(playerLevel == GameManager.Instance.experienceManager.experienceTable.Count)
        {
            levelText.text = "MAX";
            levelProgressBarMask.fillAmount = 1.0f;
        }
        else
        {
            levelText.text = "Lvl " + playerLevel.ToString();
            int previousLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel-1);
            int currentLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel);
            int experienceNeededToReachNextLevel = currentLevelAccumulatedExperience - previousLevelAccumulatedExperience;
            //the experience of the player left after progressed past the previous level
            int currentPlayerExperienceIntoLevel = GameManager.Instance.player.Experience - previousLevelAccumulatedExperience;
            float experienceRatio = (float)currentPlayerExperienceIntoLevel / (float)experienceNeededToReachNextLevel;
            levelProgressBarMask.fillAmount = experienceRatio;
        }
    }

    public void UpdateHealthPoints()
    {
        float healthPointsRatio = GameManager.Instance.player.healthPoints / GameManager.Instance.player.maxHealthPoints;
        healthBarMask.fillAmount = healthPointsRatio;
    }

    public void UpdateStatusText()
    {
        Protection protection = GameManager.Instance.player.GetArmorInfo();

        string statusText = null;

        string strengthText = protection.GetTotalStrengthLevel() != 0 ? "Strength " + protection.GetTotalStrengthLevel() : null;
        string speedText = protection.GetTotalSpeedLevel() != 0 ? "Speed " + protection.GetTotalSpeedLevel() : null;
        string bleedingText = protection.GetTotalBleedingResistanceLevel() < 0 ? "Bleeding " + Mathf.Abs(protection.GetTotalBleedingResistanceLevel()) : null;
        string elementText = protection.GetTotalElementResistanceLevel() < 0 ? "Element " + Mathf.Abs(protection.GetTotalElementResistanceLevel()) : null;

        statusText += strengthText + " " + speedText  + " " +  bleedingText  + " " +  elementText;
        statusText = statusText.Trim();

        statusInfoText.text = statusText;
    }

    public void Show()
    {
        animator.SetTrigger("Show");
        GameManager.Instance.IsHUDShown = true;
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
        GameManager.Instance.IsHUDShown = false;
    }

    public void AddToPouchSlot(Potion potion, int amount)
    {
        pouchSlots.First(x => !x.IsOccupied).AddToPouchSlot(potion, amount);
    }

    public void RemoveFromPouchSlot(string equipmentID)
    {
        pouchSlots.First(x => x.Equipment.equipmentID == equipmentID).RemoveFromPouchSlot();
    }

    public void UpdatePouchSlot(string equipmentID, int amount)
    {
        PouchSlot pouchSlot = pouchSlots.First(x => x.Equipment.equipmentID == equipmentID);
        if(amount <= 0)
            RemoveFromPouchSlot(equipmentID);
        else
            pouchSlot.UpdateAmount(amount);
    }

    public void UnlockPouchSlot()
    {
        Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);
        for(int i = 0; i < pouchInventory.maxLevel; i++)
        {
            if(i < pouchInventory.InventoryLevel)
                pouchSlots[i].UnlockSlot();
            else
                pouchSlots[i].LockSlot();
        }
    }

    public void UpdateBagSprite(int playerLevel)
    {   
        if(_currentBagLevel >= bagChangeLevelRequirements.Length)
            _currentBagLevel = bagChangeLevelRequirements.Length -1;

        if(playerLevel >= bagChangeLevelRequirements[_currentBagLevel])
        {
            bagSprite.sprite = bagSprites[_currentBagLevel];
            _currentBagLevel++;
        }
    }
}
