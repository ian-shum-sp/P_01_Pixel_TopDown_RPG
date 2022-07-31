using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public Animator _genderSelectionanimator;
    public Animator _nameInputAnimator;
    public Animator _warningAnimator;
    public TMP_InputField inputField;
    public Sprite[] playerSprites;

    private void Awake() 
    {
        Invoke("ShowGenderSelection", 1.0f);
    }

    private void ShowGenderSelection()
    {
        GameManager.Instance.IsBlockGameActions = true;
        _genderSelectionanimator.SetTrigger("Show");
    }

    private void ShowNameInput()
    {
        _nameInputAnimator.SetTrigger("Show");
    }

    private void HideWarning()
    {
        _warningAnimator.SetTrigger("Hide");
    }

    private void UpdatePlayerGenderAndGuide(Common.PlayerGender playerGender)
    {
        GameManager.Instance.player.SetPlayerSprite(playerSprites[(int)playerGender]);
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
            _warningAnimator.SetTrigger("Show");
            Invoke("HideWarning", 1.5f);
        }
        else
        {
            InitializePlayerStats();
            _nameInputAnimator.SetTrigger("Hide");
            GameManager.Instance.ShowHUD();
            GameManager.Instance.ShowRunningDialog(Common.NPCType.GUIDE);
        }
    }

    private void InitializePlayerStats()
    {
        GameManager.Instance.player.healthPoints = 20.0f;
        GameManager.Instance.player.maxHealthPoints = 20.0f;
        GameManager.Instance.player.Experience = 0;
        GameManager.Instance.player.Name = inputField.text;
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.ARMOR, 1);
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.WEAPON, 1);
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.POTION, 1);
        GameManager.Instance.player.InitializeInventory(Common.InventoryType.POUCH, 1);
    }   
}
