using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Slot : MonoBehaviour
{
    #region class members
    protected bool _isUnlocked;
    protected bool _isEquipped;
    protected bool _isOccupied;
    #endregion
    
    #region accessors
    public bool IsUnlocked
    {
        get { return _isUnlocked; }
        set { _isUnlocked = value; }
    }
    public bool IsEquipped
    {
        get { return _isEquipped; }
        set { _isEquipped = value; }
    }
    public bool IsOccupied
    {
        get { return _isOccupied; }
        set { _isOccupied = value; }
    }
    #endregion

    protected virtual void EmptySlot()
    {
        _isOccupied = false;
        _isEquipped = false;
    }

    public virtual void LockSlot()
    {
        _isUnlocked = false;
    }

    public virtual void UnlockSlot()
    {
        _isUnlocked = true;
    }
}
