using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    private SpriteRenderer _spriteRenderer;
    public Common.NPCType nPCType;
    public Sprite nPCSprite;
    public string nPCName;
    [TextArea]
    public string[] dialogs;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.UpdateNPCManager(this);
    }

    protected override void Interact()
    {
        base.Interact();
        //do something -> maybe show dialog, shop etc

        switch(nPCType)
        {
            case Common.NPCType.SIGN:
            {
                GameManager.Instance.ShowFullDialog(nPCType);
                break;
            }  
            case Common.NPCType.WARNING_SIGN:
            {
                GameManager.Instance.ShowFullDialog(nPCType, Color.red);
                break;
            }   
            default: 
                break;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void TryInteract()
    {
        if(!_isInteractable)
            return;
        
        Interact();
    }
}
