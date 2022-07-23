using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private bool _isActive = false;
    private int _currentNPCDialogIndex = 0;
    private string _currentNPCName;
    private List<string> _currentNPCDialogs = new List<string>();
    public TextMeshProUGUI dialogHeaderText;
    public TextMeshProUGUI dialogText;
    public Animator animator;
    public TextMeshProUGUI dialogTextContinue;

    private void Awake() 
    {
        dialogTextContinue.gameObject.SetActive(false);
    }

    private void Update() 
    {
        if(_isActive && Input.GetKeyDown(KeyCode.Return))
        {
            if(_currentNPCDialogIndex < _currentNPCDialogs.Count)
            {
                UpdateDialog();
                _currentNPCDialogIndex++;
            }
            else
            {
                Hide();
            }
        }
    }

    public void Show(Enums.NPCName nPCName)
    {
        _isActive = true;
        _currentNPCName = Enums.GetNPCName(nPCName);
        _currentNPCDialogs = Enums.GetNPCDialogs(nPCName);
        _currentNPCDialogIndex = 0;
        if(nPCName == Enums.NPCName.GUIDE)
            dialogTextContinue.gameObject.SetActive(true);
        UpdateDialog();
        _currentNPCDialogIndex++;
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        _isActive = false;
        if(dialogTextContinue.gameObject.activeInHierarchy)
            dialogTextContinue.gameObject.SetActive(false);
        animator.SetTrigger("Hide");
    }

    public void UpdateDialog()
    {
        dialogHeaderText.text = _currentNPCName;
        dialogText.text = _currentNPCDialogs[_currentNPCDialogIndex];
    }
}