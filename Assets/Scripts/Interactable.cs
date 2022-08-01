using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Collidable
{
    protected bool _isInteractable = false;

    protected override void OnCollide(Collider2D collider)
    {
        _isInteractable = true;
    }

    protected virtual void Interact()
    {
        _isInteractable = false;
    }
    
    public void ResetInteractability()
    {
        _isInteractable = false;
    }
}
