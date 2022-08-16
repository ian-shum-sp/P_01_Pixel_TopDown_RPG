using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowDeathMenu()
    {
        _animator.SetTrigger("Show");
        GameManager.Instance.IsDeathMenuShown = true;
    }
    public void HideDeathMenu()
    {
        _animator.SetTrigger("Hide");
        GameManager.Instance.IsDeathMenuShown = false;
    }
}
