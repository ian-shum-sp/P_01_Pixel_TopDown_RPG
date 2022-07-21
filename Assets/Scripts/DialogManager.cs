using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogHeaderText;
    public TextMeshProUGUI dialogText;
    public Animator animator;

    private void Update() 
    {
        if(Input.GetKey(KeyCode.Return))
            Hide();
    }

    public void Show(string headerText, string text)
    {
        dialogHeaderText.text = headerText;
        dialogText.text = text;
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }
}
