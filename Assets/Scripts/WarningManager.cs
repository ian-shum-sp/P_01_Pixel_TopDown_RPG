using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningManager : MonoBehaviour
{
    private bool _isContinueBlockGameAction = false;
    public TextMeshProUGUI warningText;
    public Animator animator;

    private void HideWarning()
    {
        animator.SetTrigger("Hide");
        if(_isContinueBlockGameAction)
            GameManager.Instance.IsBlockGameActions = true;
        else
            GameManager.Instance.IsBlockGameActions = false;
    }

    public void Show(string text, bool isContinueBlockGameAction)
    {
        _isContinueBlockGameAction = isContinueBlockGameAction;
        warningText.text = text;
        animator.SetTrigger("Show");
        Invoke("HideWarning", 1.5f);
    }
}
