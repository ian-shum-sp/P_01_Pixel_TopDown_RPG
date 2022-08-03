using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private float _invulnerableDuration = 0.8f;
    private float _lastInvulnerableTime;
    private float _debuffDamageInterval = 2.0f;
    private float _startBleedingTime;
    private float _lastBleedingTime;
    private float _startElementBlightedTime;
    private float _lastElementBlightedTime;
    protected int _currentBleedingResistanceLevel = 0;
    protected int _currentElementResistanceLevel = 0;
    protected bool _isBleeding = false;
    protected bool _isElementBlighted = false;
    protected float _originalKnockbackRecoverySpeed;
    protected Protection _currentProtection = new Protection();
    protected Damage _currentDamage = new Damage();
    protected Vector3 _knockbackDirection;
    protected bool _isAttacked;
    public float healthPoints;
    public float maxHealthPoints;
    public float knockbackRecoverySpeed = 0.2f;

    private void StartBleeding()
    {
        _startBleedingTime = Time.time;
        Bleed();
    }

    private void StartGetBlighted()
    {
        _startElementBlightedTime = Time.time;
        GetBlighted();
    }

    //bleeding damage = bleeding level
    private void Bleed()
    {
        UpdateSpeed();
        _isBleeding = true;
        _lastBleedingTime = Time.time;
        float damage = (float)Mathf.Abs(_currentBleedingResistanceLevel);
        healthPoints -= damage;
        GameManager.Instance.ShowFloatingText(damage.ToString(), 10, Color.red, transform.position, Vector3.zero, 0.5f);
    }

    //element damage = element level
    private void GetBlighted()
    {
        _isElementBlighted = true;
        _lastElementBlightedTime = Time.time;
        float damage = (float)Mathf.Abs(_currentElementResistanceLevel);
        healthPoints -= damage;
        GameManager.Instance.ShowFloatingText(damage.ToString(), 10, Color.red, transform.position, Vector3.zero, 0.5f);
    }

    private void StopBleeding()
    {
        _isBleeding = false;
        _currentProtection.RemoveAppliedDebuff(Common.Debuff.BLEEDING);
        UpdateSpeed();
        GameManager.Instance.UpdateStatusInfo();
        GameManager.Instance.UpdatePlayerMenuEquipmentInfo();
    }

    private void StopElementBlight()
    {
        _isElementBlighted = false;
        _currentProtection.RemoveAppliedDebuff(Common.Debuff.ELEMENT);
        GameManager.Instance.UpdateStatusInfo();
        GameManager.Instance.UpdatePlayerMenuEquipmentInfo();
    }

    protected virtual void Start()
    {
        _originalKnockbackRecoverySpeed = knockbackRecoverySpeed;
        _currentProtection.Initialize();
        _currentDamage.Initialize();
    }

    protected virtual void Update()
    {
        if(_isBleeding)
        {
            if(Time.time - _lastBleedingTime > _debuffDamageInterval)
                Bleed();

            if(Time.time - _startBleedingTime > _currentProtection.GetDebuffDuration(Common.Debuff.BLEEDING))
                StopBleeding();
        }

        if(_isElementBlighted)
        {
            if(Time.time - _lastElementBlightedTime > _debuffDamageInterval)
                GetBlighted();

            if(Time.time - _startElementBlightedTime > _currentProtection.GetDebuffDuration(Common.Debuff.ELEMENT))
                StopElementBlight();
        }
    }

    protected virtual void ReceiveDamage(Damage damage)
    {
        if(Time.time - _lastInvulnerableTime > _invulnerableDuration)
        {
            _isAttacked = true;
            _lastInvulnerableTime = Time.time;


            //positive implies buff
            //negative implies debuff
            bool isRefreshBleeding = _currentProtection.ApplyDebuffLevel(damage.weaponDebuffs[0], damage.weaponDebuffsLevels[0]);
            _currentBleedingResistanceLevel = _currentProtection.GetTotalBleedingResistanceLevel();
            if(_currentBleedingResistanceLevel < 0 && isRefreshBleeding)
                StartBleeding();

            _currentProtection.ApplyDebuffLevel(damage.weaponDebuffs[1], damage.weaponDebuffsLevels[1]);
            int knockbackLevel = _currentProtection.GetTotalKnockbackResistanceLevel();
            UpdateKnockbackRecoverySpeed();
            if(knockbackLevel < 0)
            {
                float knockbackForce = damage.knockbackForce;
                if(Mathf.Abs(knockbackLevel) == 1)
                    knockbackForce *= 1.1f;
                if(Mathf.Abs(knockbackLevel) == 2)
                    knockbackForce *= 1.25f;
                else if(Mathf.Abs(knockbackLevel) == 3)
                    knockbackForce *= 1.5f;

                _knockbackDirection = (transform.position - damage.origin).normalized * knockbackForce;
            }

            bool isRefreshElement = _currentProtection.ApplyDebuffLevel(damage.weaponDebuffs[2], damage.weaponDebuffsLevels[2]);
            _currentElementResistanceLevel = _currentProtection.GetTotalElementResistanceLevel();
            if(_currentElementResistanceLevel < 0 && isRefreshElement)
                StartGetBlighted();

            GameManager.Instance.UpdateStatusInfo();
            //calculate effective damage after element debuff and armor reduction
            float effectiveDamage = (float)damage.damagePoints;
            if(_isElementBlighted)
            {
                int elementLevel = Mathf.Abs(_currentElementResistanceLevel);
                if(elementLevel == 1 || elementLevel == 2)
                    effectiveDamage *= 0.9f;
                else
                    effectiveDamage *= 0.8f;
            }
            effectiveDamage *= (1.0f - ((float)_currentProtection.armorPoints/100.0f));
            healthPoints -= damage.damagePoints;
            GameManager.Instance.ShowFloatingText(damage.damagePoints.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

            if(healthPoints <= 0)
            {
                healthPoints = 0;
                Death();
            }
        }
    }

    protected void UpdateKnockbackRecoverySpeed()
    {
        int totalKnockbackResistanceLevel = _currentProtection.GetTotalKnockbackResistanceLevel();
        if(totalKnockbackResistanceLevel > 0)
            knockbackRecoverySpeed = _originalKnockbackRecoverySpeed + (totalKnockbackResistanceLevel * 0.2f);
        else
            knockbackRecoverySpeed = _originalKnockbackRecoverySpeed;
    }

    protected virtual void UpdateDamage()
    {

    }

    protected virtual void UpdateSpeed()
    {

    }

    protected virtual void Death()
    {

    }
}
