using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region class members
    private SpriteRenderer _spriteRenderer;
    private float _healthPoints;
    private float _maxHealthPoints;
    private int _experience;
    private string _name;

    #endregion
    
    #region accessors
    public float HealthPoints
    {
        get { return _healthPoints; }
        set { _healthPoints = value; }
    }

    public float MaxHealthPoints
    {
        get { return _maxHealthPoints; }
        set { _maxHealthPoints = value; }
    }

    public int Experience
    {
        get { return _experience; }
        set { _experience = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    #endregion

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPlayerSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private void FixedUpdate()
    {
        
    }
}
