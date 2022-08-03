using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PouchSlot : ActivatableSlot
{
    private float _activateTime;
    private float _cooldownStartTime;
    private bool _isActivated = false;
    private bool _isOnCooldown = false;
    private Potion _potion;
    public Image pouchSlotImage;
    public Image potionImage;
    public TextMeshProUGUI potionAmountText;
    public Image cooldownMask;

    private void Update()
    {
        if(_isActivated)
        {
            if(Time.time - _activateTime > _potion.duration)
            {
                _isActivated = false;
                _isOnCooldown = true;
                _cooldownStartTime = Time.time;
                GameManager.Instance.player.RemovePotionEffect(_potion);
            }
        }

        if(_isOnCooldown)
        {
            if(Time.time - _cooldownStartTime > _potion.cooldown)
            {
                _isOnCooldown = false;
            }
            else
            {
                float timeElapsed = Time.time - _cooldownStartTime;
                int cooldownRemainingTime = Mathf.CeilToInt(_potion.cooldown - timeElapsed);
                float ratio = cooldownRemainingTime / _potion.cooldown;
                cooldownMask.fillAmount = ratio;
            }
        }
    }

    protected override void ResetSlot()
    {
        base.ResetSlot();
        potionImage.sprite = null;
        potionImage.color = Common.UnoccupiedSlotImageBackgroundColor;
        potionAmountText.text = null;
    }

    protected override void ActivateSlot()
    {
        _isActivated = true;
        _isOnCooldown = false;
        _activateTime = Time.time;
        cooldownMask.fillAmount = 1.0f;
        Potion potion = _equipment as Potion;
        GameManager.Instance.player.AddPotionEffect(potion);
    }

    public override void LockSlot()
    {
        base.LockSlot();
        pouchSlotImage.color = Common.LockedSlotColor;
        ResetSlot();
    }

    public override void UnlockSlot()
    {
        base.UnlockSlot();
        pouchSlotImage.color = Common.UnlockedSlotColor;
        ResetSlot();
    }

    public void TryUsePotion()
    {
        if(!_isUnlocked)
            return;

        if(!_isOccupied)
        {
            GameManager.Instance.ShowFloatingText("No potions equipped!", 25, Color.red, GameManager.Instance.player.transform.position, Vector3.up * 25, 3.0f);
            return;
        }

        if(!_isActivated && !_isOnCooldown)
            ActivateSlot();
        else if(_isActivated && !_isOnCooldown)
        {
            float timeElapsed = Time.time - _activateTime;
            int effectRemainingTime =  Mathf.CeilToInt(_potion.duration - timeElapsed);
            GameManager.Instance.ShowFloatingText("Potion effect still in place for " + effectRemainingTime.ToString() + " seconds!", 25, Color.yellow, GameManager.Instance.player.transform.position, Vector3.up * 25, 3.0f);
        }
        else if(!_isActivated && _isOnCooldown)
        {
            float timeElapsed = Time.time - _cooldownStartTime;
            int cooldownRemainingTime = Mathf.CeilToInt(_potion.cooldown - timeElapsed);
            GameManager.Instance.ShowFloatingText("Ready in " + cooldownRemainingTime.ToString() + " seconds!", 25, Color.red, GameManager.Instance.player.transform.position, Vector3.up * 25, 3.0f);
        } 
    }
    
    public void AddToPouchSlot(Potion potion, int amount)
    {
        base.AddToSlot(potion);
        _potion = potion;
        potionImage.sprite = _potion.equipmentSprite;
        potionImage.color = Common.OccupiedSlotImageBackgroundColor;
        potionAmountText.text = amount.ToString();
    }

    public void UpdateAmount(int amount)
    {
        potionAmountText.text = amount.ToString();
    }

    public void RemoveFromPouchSlot()
    {
        ResetSlot();
    }
}
