using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Collidable
{
    #region class members
    private Animator _animator;
    private Damage _currentDamage;
    private float _lastAttackTime;
    public bool _isAttacking = false;
    #endregion

    #region accessors
    #endregion

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    protected override void OnCollide(Collider2D collider)
    {
        if(collider.tag == "Fighter")
        {
            if(collider.name == "Player")
                return;

            collider.SendMessage("ReceiveDamage", _currentDamage);
        }
    }

    public void TryAttack(Damage damage)
    {
        if(Time.time - _lastAttackTime <= damage.cooldown)
            return;

        _lastAttackTime = Time.time;
        _animator.SetTrigger("Attack");
        _currentDamage = damage;
    }
}
