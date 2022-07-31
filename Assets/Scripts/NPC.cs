using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public Common.NPCType nPCType;
    public Sprite nPCSprite;
    public string nPCName;
    [TextArea]
    public string[] dialogs;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
