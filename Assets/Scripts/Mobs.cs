using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobs : Movable
{
    private bool _isChasing;
    private bool _isWithinAttackRange;
    private Transform _playerTransform;
    private Vector3 _startingPosition;
    private Collider2D[] _hits = new Collider2D[10];
    public ContactFilter2D filter;
    public int experience;
    public int gold;
    public float triggerLength = 0.48f;
    public float chaseLength = 0.8f;
    public EnemyWeapon enemyWeapon;
    public int damagePoints;
    public float knockbackForce;
    public float attackCooldown;
    public int attackRange;
    public Common.Debuff weaponDebuff;
    public int debuffLevel;
    public int armorPoints;
    public Common.ArmorBuff armorBuff;
    public int buffLevel;
    public BoxCollider2D detector;

    protected override void Start()
    {
        base.Start();
        _playerTransform = GameManager.Instance.player.transform;
        _startingPosition = transform.position;
        //set the info into the combat struct
        _currentDamage.isHaveWeapon = true;
        _currentDamage.isMelee = attackRange >= 60 ? false : true;
        _currentDamage.origin = enemyWeapon.transform.position;
        _currentDamage.damagePoints = (float)damagePoints;
        _currentDamage.knockbackForce = knockbackForce;
        _currentDamage.cooldown = attackCooldown;
        _currentDamage.attackRange = attackRange;
        _currentDamage.attackSpeed = Mathf.FloorToInt((2.0f - attackCooldown)*20);
        _currentDamage.SetWeaponBuffLevel(weaponDebuff, debuffLevel);
        _currentProtection.armorPoints = armorPoints;
        _currentProtection.SetArmorBuffLevel(armorBuff, buffLevel);
    }

    private void FixedUpdate()
    {
        //if there is any active dialogs or player menu shown
        if(!GameManager.Instance.IsBlockGameActions)
        {
            //if melee weapon, can move when attacking
            if(_currentDamage.isMelee)
            {
                Move();
                return;
            } 
            else
            {
                //player cannot move when he is attacking (in range weapon), if he is attacking and at the same time being attacked, knockback is applied
                if(!enemyWeapon._isAttacking)
                {
                    Move();
                    return;
                }

                if(_isAttacked)
                {
                    UpdateMotor(new Vector3(0.0f, 0.0f, 0.0f));
                    return;
                }
            }   
        }
    }

    private void Move()
    {
        //Check if the player is in range
        if(Vector3.Distance(_playerTransform.position, _startingPosition) < chaseLength)
        {
            if(Vector3.Distance(_playerTransform.position, _startingPosition) < triggerLength)
            {
                _isChasing = true;
            }

            if(_isChasing)
            {
                if(!_isWithinAttackRange)
                    UpdateMotor((_playerTransform.position - transform.position).normalized);
                else
                    enemyWeapon.TryAttack(_currentDamage);
            }
            //return to starting position
            else
            {
                UpdateMotor(_startingPosition - transform.position);
            }
        }
        //return to starting position
        else
        {
            UpdateMotor(_startingPosition - transform.position);
            _isChasing = false;
        }

        //Check if within attack range
        _isWithinAttackRange = false;
        detector.OverlapCollider(filter, _hits);
        for (int i = 0; i < _hits.Length; i++)
        {
            if(_hits[i] == null)
                continue;

            if(_hits[i].tag == "Fighter" && _hits[i].name == "Player")
            {
                if((_playerTransform.position - transform.position).x > 0.0f)
                {
                    transform.localScale = _originalSize;
                }
                else if((_playerTransform.position - transform.position).x < 0.0f)
                {
                    transform.localScale = new Vector3(_originalSize.x * -1.0f, _originalSize.y, _originalSize.z);
                }
                _isWithinAttackRange = true;
            }

            //Clean the array
            _hits[i] = null;
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.Instance.AddExperienceToPlayer(experience);
        GameManager.Instance.player.Gold += gold;
        GameManager.Instance.UpdatePlayerMenuGold();
        GameManager.Instance.ShowFloatingText("+" + experience + " experience!", 30, Color.magenta, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.24f, 0.0f), Vector3.up * 30, 2.0f);
        GameManager.Instance.ShowFloatingText("+" + gold + " gold!", 30, Color.yellow, GameManager.Instance.player.transform.position + new Vector3(0.0f, 0.16f, 0.0f), Vector3.up * 30, 2.0f);
    }
}
