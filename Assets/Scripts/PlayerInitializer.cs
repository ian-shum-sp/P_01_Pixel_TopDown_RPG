using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public Animator genderSelectionAnimator;
    public Animator nameInputAnimator;
    public TMP_InputField inputField;

    private void Awake() 
    {
        Invoke("ShowGenderSelection", 1.0f);
    }

    private void ShowGenderSelection()
    {
        GameManager.Instance.IsBlockGameActions = true;
        genderSelectionAnimator.SetTrigger("Show");
    }

    private void ShowNameInput()
    {
        nameInputAnimator.SetTrigger("Show");
    }

    private void UpdatePlayerGenderAndGuide(Common.PlayerGender playerGender)
    {
        GameManager.Instance.player.Gender = playerGender;
        GameManager.Instance.player.SetPlayerSprite();
        GameManager.Instance.UpdateGuideName(playerGender);
        Invoke("ShowNameInput", 0.25f);
    }

    public void OnMaleClicked()
    {
        UpdatePlayerGenderAndGuide(Common.PlayerGender.MALE);
    }

    public void OnFemaleClicked()
    {
        UpdatePlayerGenderAndGuide(Common.PlayerGender.FEMALE);
    }

    public void OnNameOkClicked()
    {
        if(string.IsNullOrEmpty(inputField.text) || string.IsNullOrWhiteSpace(inputField.text))
        {
            GameManager.Instance.ShowWarning("Please enter your name");
        }
        else
        {
            InitializePlayerStats();
            nameInputAnimator.SetTrigger("Hide");
            GameManager.Instance.ShowHUD();
            GameManager.Instance.InitializePlayerMenu();
            GameManager.Instance.ShowRunningDialog(Common.NPCType.GUIDE);
        }
    }

    private void InitializePlayerStats()
    {
        GameManager.Instance.player.healthPoints = 100.0f;
        GameManager.Instance.player.maxHealthPoints = 100.0f;
        GameManager.Instance.player.Experience = 0;
        GameManager.Instance.player.Name = inputField.text;
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.ARMOR, 1);
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.WEAPON, 1);
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.POTION, 1);
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.POUCH, 1);
    }   
}
