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
        _isClickedYes = true;
        if(GameManager.Instance.IsTryResetGame)
        {
            GameManager.Instance.ResetGame();
        }
    }

    public void OnNoClicked()
    {
        _isClickedYes = false;
        if(GameManager.Instance.IsTryResetGame)
        {
            GameManager.Instance.IsTryResetGame = false;
        }
    }
}
