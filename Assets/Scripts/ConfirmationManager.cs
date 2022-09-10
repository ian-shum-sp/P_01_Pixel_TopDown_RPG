using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationManager : MonoBehaviour
{
    private bool _isClickedYes = false;
    public TextMeshProUGUI confirmationText;
    public Animator animator;
    
    #region accessors
    public bool IsClickedYes
    {
        get { return _isClickedYes; }
        set { _isClickedYes = value; }
    }
    #endregion

    public void Show(string text)
    {
        _isClickedYes = false;
        confirmationText.text = text;
        animator.SetTrigger("Show");
    }

    public void OnYesClicked()
    {
        GameManager.Instance.IsBlockGameActions = false;
        _isClickedYes = true;
        if(GameManager.Instance.IsTryResetGame)
        {
            GameManager.Instance.ResetGame();
            GameManager.Instance.IsBlockGameActions = true;
            GameManager.Instance.IsTryResetGame = false;
        }

        if(GameManager.Instance.IsTryLoadMainMenu)
        {
            GameManager.Instance.ReturnToMainMenu();
            GameManager.Instance.IsBlockGameActions = true;
            GameManager.Instance.IsTryLoadMainMenu = false;
        }

        if(GameManager.Instance.IsTryExitGame)
        {
            GameManager.Instance.ExitGame();
        }
    }

    public void OnNoClicked()
    {
        GameManager.Instance.IsBlockGameActions = false;
        _isClickedYes = false;
        if(GameManager.Instance.IsTryResetGame)
        {
            GameManager.Instance.IsBlockGameActions = true;
            GameManager.Instance.IsTryResetGame = false;
        }

        if(GameManager.Instance.IsTryLoadMainMenu)
        {
            GameManager.Instance.IsBlockGameActions = true;
            GameManager.Instance.IsTryLoadMainMenu = false;
        }

        if(GameManager.Instance.IsTryExitGame)
        {
            GameManager.Instance.IsBlockGameActions = true;
            GameManager.Instance.IsTryExitGame = false;
        }
    }
}
