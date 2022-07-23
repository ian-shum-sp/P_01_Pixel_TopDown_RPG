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

    private void Awake() 
    {
        Invoke("ShowGenderSelection", 1.0f);
    }

    private void ShowGenderSelection()
    {
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

    public void OnMaleClicked()
    {
        GameManager.Instance.player.SetPlayerSprite(GameManager.Instance.playerSprites[(int)Enums.PlayerGender.MALE]);
        Invoke("ShowNameInput", 0.25f);
    }

    public void OnFemaleClicked()
    {
        GameManager.Instance.player.SetPlayerSprite(GameManager.Instance.playerSprites[(int)Enums.PlayerGender.FEMALE]);
        Invoke("ShowNameInput", 0.25f);
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
            GameManager.Instance.player.Name = inputField.text;
            _nameInputAnimator.SetTrigger("Hide");
            GameManager.Instance.ShowDialog(Enums.NPCName.GUIDE);
        }
    }
}
