using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Collidable
{
    private bool _isOpened = false;
    public Animator animator;

    private void OpenDoor()
    {
        _isOpened = true;
        animator.SetTrigger("OpenDoor");
    }

    protected override void OnCollide(Collider2D collider)
    {
        if(_isOpened)
            return;

        if(collider.name == "Player")
            OpenDoor();
    }

    
}
