using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private bool _isActive = false;
    private int _fullDialogCurrentNPCDialogIndex = 0;
    private int _runningDialogCurrentNPCDialogIndex = 0;
    private string _currentNPCName;
    private string[] _currentNPCDialogs = new string[20];
    private Color _textColor = Color.black;
    private bool _isShowAllDialogs = true;
    public TextMeshProUGUI dialogHeaderText;
    public TextMeshProUGUI dialogText;
    public Animator animator;

    private void Update() 
    {
        if(_isActive && _isShowAllDialogs && Input.GetKeyDown(KeyCode.Return))
        {
            if(_fullDialogCurrentNPCDialogIndex < _currentNPCDialogs.Length)
            {
                UpdateDialog();
            }
            else
            {
                Hide();
            }
        }
        else if(_isActive && !_isShowAllDialogs && Input.GetKeyDown(KeyCode.Return))
        {
            Hide();
        }
    }

    private void Show(Color? color)
    {
        _isActive = true;
        
        if(color != null)
            _textColor = (Color)color;
        else
            _textColor = Color.black;
        UpdateDialog();
        animator.SetTrigger("Show");
    }

    public void ShowFullDialog(Common.NPCType nPCType, Color? color)
    {
        _isShowAllDialogs = true;
        _currentNPCName = GameManager.Instance.GetNPCName(nPCType);
        _currentNPCDialogs = GameManager.Instance.GetNPCDialogs(nPCType);
        _fullDialogCurrentNPCDialogIndex = 0;
        Show(color);
    }

    public void ShowRunningDialog(Common.NPCType nPCType, Color? color)
    {
        _isShowAllDialogs = false;
        //Check if is want to continue to show the next dialog of same NPC, if not same NPC then restart from first dialog of the new NPC
        if(_currentNPCName !=  GameManager.Instance.GetNPCName(nPCType))
        {
            _currentNPCName =  GameManager.Instance.GetNPCName(nPCType);
            _currentNPCDialogs =  GameManager.Instance.GetNPCDialogs(nPCType);
            _runningDialogCurrentNPCDialogIndex = 0;
        }
        else
        {
            if(_runningDialogCurrentNPCDialogIndex >= _currentNPCDialogs.Length)
            {
                _runningDialogCurrentNPCDialogIndex = _currentNPCDialogs.Length-1;
            }
        }
        Show(color);
    }

    public void Hide()
    {
        _isActive = false;
        animator.SetTrigger("Hide");
        GameManager.Instance.IsBlockGameActions = false;
    }

    public void UpdateDialog()
    {
        dialogHeaderText.text = _currentNPCName;
        if(_isShowAllDialogs)
        {
            dialogText.text = _currentNPCDialogs[_fullDialogCurrentNPCDialogIndex];
            _fullDialogCurrentNPCDialogIndex++;
        }
        else
        {
            dialogText.text = _currentNPCDialogs[_runningDialogCurrentNPCDialogIndex];
            _runningDialogCurrentNPCDialogIndex++;
        }
    }
}