using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowMenu()
    {
        _animator.SetTrigger("Show");
        GameManager.Instance.IsMainMenuShown = true;
    }
    public void HideMenu()
    {
        _animator.SetTrigger("Hide");
        GameManager.Instance.IsMainMenuShown = false;
    }
}
