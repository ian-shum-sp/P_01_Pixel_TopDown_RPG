using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private int _fullDialogCurrentNPCDialogIndex = 0;
    private int _runningDialogCurrentNPCDialogIndex = 0;
    private string _currentFullNPCName;
    private string _currentRunningNPCName;
    private string[] _currentFullNPCDialogs = new string[20];
    private string[] _currentRunningNPCDialogs = new string[20];
    private bool _isShowAllDialogs = true;
    public TextMeshProUGUI dialogHeaderText;
    public TextMeshProUGUI dialogText;
    public Animator animator;

    private void Update() 
    {
        if(GameManager.Instance.IsDialogShown && _isShowAllDialogs && Input.GetKeyDown(KeyCode.Return))
        {
            if(_fullDialogCurrentNPCDialogIndex < _currentFullNPCDialogs.Length)
            {
                UpdateDialog();
            }
            else
            {
                Hide();
            }
        }
        else if(GameManager.Instance.IsDialogShown && !_isShowAllDialogs && Input.GetKeyDown(KeyCode.Return))
        {
            Hide();
        }
    }

    private void UpdateDialog()
    {
        if(_isShowAllDialogs)
        {
            dialogHeaderText.text = _currentFullNPCName;
            dialogText.text = _currentFullNPCDialogs[_fullDialogCurrentNPCDialogIndex];
            _fullDialogCurrentNPCDialogIndex++;
        }
        else
        {
            dialogHeaderText.text = _currentRunningNPCName;
            dialogText.text = _currentRunningNPCDialogs[_runningDialogCurrentNPCDialogIndex];
            _runningDialogCurrentNPCDialogIndex++;
        }
    }

    private void Show()
    {
        UpdateDialog();
        animator.SetTrigger("Show");
        GameManager.Instance.IsDialogShown = true;
    }

    private void Hide()
    {
        animator.SetTrigger("Hide");
        GameManager.Instance.IsDialogShown = false;
        GameManager.Instance.IsBlockGameActions = false;
    }

    public void ShowFullDialog(string nPCID)
    {
        _isShowAllDialogs = true;
        _currentFullNPCName = GameManager.Instance.GetNPCName(nPCID);
        _currentFullNPCDialogs = GameManager.Instance.GetNPCDialogs(nPCID);
        _fullDialogCurrentNPCDialogIndex = 0;
        Show();
    }

    public void ShowRunningDialog(string nPCID, bool isReset)
    {
        _isShowAllDialogs = false;
        //Check if is want to continue to show the next dialog of same NPC, if not same NPC then restart from first dialog of the new NPC
        //Or if specify reset, then will show from first dialog again of the same npc
        if(_currentRunningNPCName != GameManager.Instance.GetNPCName(nPCID) || isReset)
        {
            _currentRunningNPCName = GameManager.Instance.GetNPCName(nPCID);
            _currentRunningNPCDialogs = GameManager.Instance.GetNPCDialogs(nPCID);
            _runningDialogCurrentNPCDialogIndex = 0;
        }
        else
        {
            if(_runningDialogCurrentNPCDialogIndex >= _currentRunningNPCDialogs.Length)
            {
                _runningDialogCurrentNPCDialogIndex = _currentRunningNPCDialogs.Length-1;
            }
        }
        Show();
    }
}