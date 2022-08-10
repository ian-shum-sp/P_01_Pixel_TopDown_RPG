using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    private SpriteRenderer _spriteRenderer;
    private Transform _playerTransform;
    private Vector3 _originalSize;
    public Common.NPCType nPCType;
    public bool isShopNPC;
    public string nPCName;
    [TextArea]
    public string[] dialogs;

    protected override void Start()
    {
        base.Start();
        _playerTransform = GameManager.Instance.player.transform;
        _originalSize = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.Instance.UpdateNPCManager(this);
    }

    protected override void Interact()
    {
        base.Interact();

        switch(nPCType)
        {
            case Common.NPCType.DUNGEON_ARMORER:
            case Common.NPCType.ENCHANTED_FOREST_ARMORER:
            case Common.NPCType.FANTASY_ARMORER:
            case Common.NPCType.DUNGEON_WEAPONSMITH:
            case Common.NPCType.ENCHANTED_FOREST_WEAPONSMITH:
            case Common.NPCType.FANTASY_WEAPONSMITH:
            case Common.NPCType.DUNGEON_POTION_BREWER:
            case Common.NPCType.ENCHANTED_FOREST_POTION_BREWER:
            case Common.NPCType.FANTASY_POTION_BREWER:
            {
                if((_playerTransform.position - transform.position).x > 0.0f)
                {
                    transform.localScale = _originalSize;
                }
                else if((_playerTransform.position - transform.position).x < 0.0f)
                {
                    transform.localScale = new Vector3(_originalSize.x * -1.0f, _originalSize.y, _originalSize.z);
                }
                GameManager.Instance.ShowFullDialog(nPCType);
                break;
            }
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

    public bool TryInteract()
    {
        if(!_isInteractable)
            return false;
        
        Interact();
        return true;
    }
}
