using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region class members
    private List<Image> pouchSlotImages = new List<Image>();
    private List<Image> potionImages = new List<Image>();
    private List<TextMeshProUGUI> potionAmountTexts = new List<TextMeshProUGUI>();
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI statusInfoText;
    public Image levelProgressBarMask;
    public Image healthBarMask;
    public GameObject[] pouchSlots;
    public Animator animator;
    #endregion
    
    #region accessors

    #endregion
    private void Start() 
    {
        for (int i = 0; i < pouchSlots.Length; i++)
        {
            pouchSlotImages.Add(pouchSlots[i].GetComponent<Image>());
            Transform potionImageTransform = pouchSlots[i].transform.Find("PotionImage");
            potionImages.Add(potionImageTransform.GetComponent<Image>());
            Transform potionAmountTextTransform = pouchSlots[i].transform.Find("PotionAmountText");
            potionAmountTexts.Add(potionAmountTextTransform.GetComponent<TextMeshProUGUI>());
        }
    }

    private void EnablePouch(int index)
    {
        pouchSlotImages[index].color = Common.EnabledSlotColor;
        ResetPouch(index);
    }

    private void DisablePouch(int index)
    {
        pouchSlotImages[index].color = Common.DisabledSlotColor;
        ResetPouch(index);
    }

    private void ResetPouch(int index)
    {
        potionImages[index].sprite = null;
        potionImages[index].color = Common.UnequippedSlotImageBackgroundColor;
        potionAmountTexts[index].text = null;
    }

    public void InitializeHUD()
    {
        int playerLevel = Common.CalculateLevelFromExperience();
        levelText.text = playerLevel.ToString();

        int previousLevelAccumulatedExperience = Common.GetAccumulatedExperienceOfLevel(playerLevel-1);
        int currentLevelAccumulatedExperience = Common.GetAccumulatedExperienceOfLevel(playerLevel);
        int experienceNeededToReachNextLevel = currentLevelAccumulatedExperience - previousLevelAccumulatedExperience;
        //the experience of the player left after progressed past the previous level
        int currentPlayerExperienceIntoLevel = GameManager.Instance.player.Experience - previousLevelAccumulatedExperience;
        float experienceRatio = (float)currentPlayerExperienceIntoLevel / (float)experienceNeededToReachNextLevel;
        levelProgressBarMask.fillAmount = experienceRatio;

        playerNameText.text = GameManager.Instance.player.Name;
        statusInfoText.text = null;

        float healthPointsRatio = GameManager.Instance.player.healthPoints / GameManager.Instance.player.maxHealthPoints;
        healthBarMask.fillAmount = healthPointsRatio;

        Inventory playerPouch = GameManager.Instance.player.GetInventoryInfo(Common.InventoryType.POUCH);

        for(int i = 0; i < playerPouch.maxLevel; i++)
        {
            if(i < playerPouch.InventoryLevel)
                EnablePouch(i);
            else
                DisablePouch(i);
        }
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }
}
