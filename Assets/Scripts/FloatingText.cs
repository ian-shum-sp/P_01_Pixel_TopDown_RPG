using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    #region class members
    private bool _isActive;
    private GameObject _target;
    private TextMeshProUGUI _text;
    private Vector3 _motion;
    private float _showDuration;
    private float _lastShownTime;
    #endregion
    
    #region accessors
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }
    public TextMeshProUGUI Text
    {
        get { return _text; }
        set { _text = value; }
    }
    public Vector3 Motion
    {
        get { return _motion; }
        set { _motion = value; }
    }
    public float ShowDuration
    {
        get { return _showDuration; }
        set { _showDuration = value; }
    }
    #endregion
    
    public void Show()
    {
        _isActive = true;
        _lastShownTime = Time.time;
        _target.SetActive(true);
    }

    public void Hide()
    {
        _isActive = false;
        _target.SetActive(false);
    }

    //UpdateFloatingText is being called at the FloatingTextManager Update Function
    public void UpdateFloatingText()
    {
        if(!_isActive)
            return;

        if(Time.time - _lastShownTime > _showDuration)
        {
            Hide();
        }

        _target.transform.position += Motion * Time.deltaTime;
    }
}
