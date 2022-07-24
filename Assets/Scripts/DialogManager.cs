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
    private Color _textColor = Color.black;
    public TextMeshProUGUI dialogHeaderText;
    public TextMeshProUGUI dialogText;
    public Animator animator;

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

    public void Show(Enums.NPCName nPCName, Color? color = null)
    {
        _isActive = true;
        _currentNPCName = Enums.GetNPCName(nPCName);
        _currentNPCDialogs = Enums.GetNPCDialogs(nPCName);
        _currentNPCDialogIndex = 0;
        if(nPCName == Enums.NPCName.GUIDE)
            GameManager.Instance.player.IsActive = true;
        if(color != null)
            _textColor = (Color)color;
        UpdateDialog();
        _currentNPCDialogIndex++;
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        _isActive = false;
        GameManager.Instance.player.IsActive = true;
        _textColor = Color.black;
        animator.SetTrigger("Hide");
    }

    public void UpdateDialog()
    {
        dialogHeaderText.text = _currentNPCName;
        dialogText.text = _currentNPCDialogs[_currentNPCDialogIndex];
    }
}