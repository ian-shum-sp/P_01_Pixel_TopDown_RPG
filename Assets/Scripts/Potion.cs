using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Equipment
{
    public Common.PotionBuff potionBuff;
    public int buffLevel;
    public float duration;
    public float cooldown;
    public int maxNumberInPouch;
}
