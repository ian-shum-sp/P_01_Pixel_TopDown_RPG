using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
   #region class members
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI statusInfoText;
    public Image levelProgressBarMask;
    public Image healthBarMask;
    public PouchSlot[] pouchSlots;
    public Animator animator;
    #endregion
    
    #region accessors

    #endregion
   
    public void InitializeHUD()
    {
        int playerLevel = GameManager.Instance.GetPlayerLevel();
        levelText.text = "Lvl " + playerLevel.ToString();

        int previousLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel-1);
        int currentLevelAccumulatedExperience = GameManager.Instance.GetAccumulatedExperienceOfLevel(playerLevel);
        int experienceNeededToReachNextLevel = currentLevelAccumulatedExperience - previousLevelAccumulatedExperience;
        //the experience of the player left after progressed past the previous level
        int currentPlayerExperienceIntoLevel = GameManager.Instance.player.Experience - previousLevelAccumulatedExperience;
        float experienceRatio = (float)currentPlayerExperienceIntoLevel / (float)experienceNeededToReachNextLevel;
        levelProgressBarMask.fillAmount = experienceRatio;

        playerNameText.text = GameManager.Instance.player.Name;
        statusInfoText.text = null;

        float healthPointsRatio = GameManager.Instance.player.healthPoints / GameManager.Instance.player.maxHealthPoints;
        healthBarMask.fillAmount = healthPointsRatio;

        Inventory pouchInventory = GameManager.Instance.player.GetInventory(Common.InventoryType.POUCH);

        for(int i = 0; i < pouchInventory.maxLevel; i++)
        {
            if(i < pouchInventory.InventoryLevel)
                pouchSlots[i].UnlockSlot();
            else
                pouchSlots[i].LockSlot();
        }
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }
}
