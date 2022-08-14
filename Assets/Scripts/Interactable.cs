using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Collidable
{
    protected bool _isInteractable = false;

    protected override void OnCollide(Collider2D collider)
    {
        if(collider.name == "Player")
            _isInteractable = true;      
    }

    protected virtual void Interact()
    {

    }
    
    public void ResetInteractability()
    {
        _isInteractable = false;
    }
}
