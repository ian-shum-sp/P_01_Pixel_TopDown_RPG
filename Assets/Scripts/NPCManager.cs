using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Sprite[] guideSprites;
    public List<NPC> gameNPCs;

    public string GetNPCName(Common.NPCType nPCType)
    {
        NPC gameNPC = gameNPCs.FirstOrDefault(x => x.nPCType == nPCType);

        if(gameNPC != null)
            return gameNPC.nPCName;

        return null;
    }

    public string[] GetNPCDialogs(Common.NPCType nPCType)
    {
        NPC gameNPC = gameNPCs.FirstOrDefault(x => x.nPCType == nPCType);

        if(gameNPC != null)
            return gameNPC.dialogs;

        return new string[20];
    }

    public void UpdateGuideName(Common.PlayerGender playerGender)
    {
        if(playerGender == Common.PlayerGender.MALE)
        {
            gameNPCs[(int)Common.NPCType.GUIDE].nPCName = "Marley";
            gameNPCs[(int)Common.NPCType.GUIDE].SetSprite(guideSprites[(int)Common.PlayerGender.FEMALE]);
        }
        else
        {
            gameNPCs[(int)Common.NPCType.GUIDE].nPCName = "Brad";
            gameNPCs[(int)Common.NPCType.GUIDE].SetSprite(guideSprites[(int)Common.PlayerGender.MALE]);
        }
    }
}
