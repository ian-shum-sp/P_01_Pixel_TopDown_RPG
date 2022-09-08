using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Slot : MonoBehaviour
{
    #region class members
    protected Equipment _equipment;
    protected bool _isUnlocked;
    protected bool _isOccupied;
    protected bool _isSelected = false;
    #endregion

    #region accessors
    public bool IsUnlocked
    {
        get { return _isUnlocked; }
        set { _isUnlocked = value; }
    }
    public bool IsOccupied
    {
        get { return _isOccupied; }
        set { _isOccupied = value; }
    }
    public Equipment Equipment
    {
        get { return _equipment; }
        set { _equipment = value; }
    }
    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }
    #endregion

    protected virtual void ResetSlot()
    {
        _isOccupied = false;
        _equipment = null;
    }

    public virtual void LockSlot()
    {
        _isUnlocked = false;
    }

    public virtual void UnlockSlot()
    {
        _isUnlocked = true;
    }

    public virtual void AddToSlot(Equipment equipment)
    {
        _isOccupied = true;
        _equipment = equipment;
    }

    public virtual void RemoveFromSlot()
    {
        _isOccupied = false;
        _equipment = null;
    }

    public virtual void SelectSlot()
    {
        _isSelected = true;
    }

    public virtual void DeselectSlot()
    {
        _isSelected = false;
    }
}
