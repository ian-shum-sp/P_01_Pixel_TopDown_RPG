using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderSelection : MonoBehaviour
{
    private Animator _animator;
    public Animator _nameInputAnimator;

    private void Awake() 
    {
        _animator = GetComponent<Animator>();
        Invoke("Show", 1.0f);
    }

    private void Show()
    {
        _animator.SetTrigger("Show");
    }

    public void OnMaleClicked()
    {
        GameManager.Instance.player.SetPlayerSprite(GameManager.Instance.playerSprites[(int)Enums.PlayerGender.MALE]);
        Invoke("ShowNameInput", 0.75f);
    }

    public void OnFemaleClicked()
    {
        GameManager.Instance.player.SetPlayerSprite(GameManager.Instance.playerSprites[(int)Enums.PlayerGender.FEMALE]);
        Invoke("ShowNameInput", 0.75f);
    }

    public void ShowNameInput()
    {
        _nameInputAnimator.SetTrigger("Show");
    }
}
