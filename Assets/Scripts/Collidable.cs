using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D _boxCollider;
    private Collider2D[] _hits = new Collider2D[10];

    //Will always be called in subclasses
    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() 
    {
        _boxCollider.OverlapCollider(filter, _hits);
        for (int i = 0; i < _hits.Length; i++)
        {
            //If the current collider is empty, move to the check the next one
            if(_hits[i] == null)
                continue;

            OnCollide(_hits[i]);
            
            //Clean the array
            _hits[i] = null;
        }   
    }

    protected virtual void OnCollide(Collider2D collider)
    {
    }
}
