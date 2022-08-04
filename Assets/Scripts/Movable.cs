using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Fighter
{
    private Vector3 _moveDelta;
    private RaycastHit2D _hit;
    private BoxCollider2D _boxCollider;
    protected Vector3 _originalSize;
    protected float _originalXSpeed;
    protected float _originalYSpeed;
    public float xSpeed = 1.0f;
    public float ySpeed = 0.75f;

    protected override void Start()
    {
        base.Start();
        _originalSize = transform.localScale;
        _originalXSpeed = xSpeed;
        _originalYSpeed = ySpeed;
        _currentProtection.movementSpeed = Mathf.FloorToInt(xSpeed * 300.0f);
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected void UpdateMotor(Vector3 input)
    {
        _moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0.0f);

        //swap sprite direction (right to left)
        if(_moveDelta.x > 0.0f)
        {
            transform.localScale = _originalSize;
        }
        else if(_moveDelta.x < 0.0f)
        {
            transform.localScale = new Vector3(_originalSize.x * -1.0f, _originalSize.y, _originalSize.z);
        }

        _moveDelta += _knockbackDirection;

        if(_knockbackDirection == Vector3.zero)
            _isAttacked = false;

        //reduce knockback force every frame, based off recovery speed
        _knockbackDirection = Vector3.Lerp(_knockbackDirection, Vector3.zero, knockbackRecoverySpeed);
        
        //check if player collides with Blocking and Actor layer by casting a box in the expected position first, if colliders is null, means no collision (for x)
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0.0f, new Vector2(_moveDelta.x, 0.0f), Mathf.Abs(_moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking","Character"));
        if(_hit.collider == null)
        {
            //move
            transform.Translate(_moveDelta.x * Time.deltaTime, 0.0f, 0.0f);
        }

        //check if player collides with Blocking and Actor layer by casting a box in the expected position first, if colliders is null, means no collision (for y)
        _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0.0f, new Vector2(0.0f, _moveDelta.y), Mathf.Abs(_moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking","Character"));
        if(_hit.collider == null)
        {
            //move
            transform.Translate(0.0f, _moveDelta.y * Time.deltaTime, 0.0f);
        }
    }

    protected override void UpdateSpeed()
    {
        int speedLevel = _currentProtection.GetTotalSpeedLevel();
        if(speedLevel== 1 || speedLevel == 2)
        {
            xSpeed = _originalXSpeed * 1.1f;
            ySpeed = _originalYSpeed * 1.1f;
        }
        else if(speedLevel == 3)
        {
            xSpeed = _originalXSpeed * 1.2f;
            ySpeed = _originalYSpeed * 1.2f;
        }
        else
        {
            xSpeed = _originalXSpeed;
            ySpeed = _originalYSpeed;
        }

        if(_isBleeding)
        {
            int bleedingLevel = Mathf.Abs(_currentBleedingResistanceLevel);
            if(bleedingLevel == 1 || bleedingLevel == 2)
            {
                xSpeed *= 0.9f;
                ySpeed *= 0.9f;
            }
            else
            {
                xSpeed *= 0.8f;
                ySpeed *= 0.8f;
            }
        }

        _currentProtection.movementSpeed = Mathf.FloorToInt(xSpeed * 300.0f);
    }
}
