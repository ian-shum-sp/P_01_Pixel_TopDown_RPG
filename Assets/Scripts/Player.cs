using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region class members
    private Vector3 _originalSize;
    private SpriteRenderer _spriteRenderer;
    private int _experience;
    private string _name;
    private bool _isActive;
    private Vector3 _moveDelta;
    private BoxCollider2D _boxCollider;
    private RaycastHit2D _hit;
    public float healthPoints;
    public float maxHealthPoints;
    public float xSpeed;
    public float ySpeed;
    #endregion
    
    #region accessors
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
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    #endregion

    private void Start()
    {
        _originalSize = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void SetPlayerSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(_isActive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    private void UpdateMotor(Vector3 input)
    {
        _moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //swap sprite direction (right to left)
        if(_moveDelta.x > 0)
        {
            transform.localScale = _originalSize;
        }
        else if(_moveDelta.x < 0)
        {
            transform.localScale = new Vector3(_originalSize.x * -1, _originalSize.y, _originalSize.z);
        }
        
        //check if player collides with Blocking and Actor layer by casting a box in the expected position first, if colliders is null, means no collision (for x)
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(_moveDelta.x, 0), Mathf.Abs(_moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking","Actor"));
        if(_hit.collider == null)
        {
            //move
            transform.Translate(_moveDelta.x * Time.deltaTime, 0, 0);
        }

        //check if player collides with Blocking and Actor layer by casting a box in the expected position first, if colliders is null, means no collision (for y)
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(0, _moveDelta.y), Mathf.Abs(_moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking","Actor"));
        if(_hit.collider == null)
        {
            //move
            transform.Translate(0, _moveDelta.y * Time.deltaTime, 0);
        }
    }
}
