using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Sprite[] guideSprites;
    private List<NPC> nPCs = new List<NPC>();

    private void Update()
    {
        foreach(NPC nPC in nPCs)
        {
            if(Input.GetKeyDown(KeyCode.I))
                nPC.TryInteract();

            nPC.ResetInteractability();
        }
    }

    public void AddNPC(NPC nPCToAdd)
    {
        NPC nPC = nPCs.FirstOrDefault(x => x.nPCType == nPCToAdd.nPCType);

        if(nPC == null)
            nPCs.Add(nPCToAdd);
    }

    public string GetNPCName(Common.NPCType nPCType)
    {
        NPC nPC = nPCs.FirstOrDefault(x => x.nPCType == nPCType);

        if(nPC != null)
            return nPC.nPCName;

        return null;
    }

    public string[] GetNPCDialogs(Common.NPCType nPCType)
    {
        NPC nPC = nPCs.FirstOrDefault(x => x.nPCType == nPCType);

        if(nPC != null)
            return nPC.dialogs;

        return new string[20];
    }

    public void UpdateGuide(Common.PlayerGender playerGender)
    {
        int guideIndex = nPCs.FindIndex(x => x.nPCType == Common.NPCType.GUIDE);

        if(guideIndex < 0)
            return;

        if(playerGender == Common.PlayerGender.MALE)
        {
            nPCs[guideIndex].nPCName = "Marley";
            nPCs[guideIndex].SetSprite(guideSprites[(int)Common.PlayerGender.FEMALE]);
        }
        else
        {
            nPCs[guideIndex].nPCName = "Brad";
            nPCs[guideIndex].SetSprite(guideSprites[(int)Common.PlayerGender.MALE]);
        }
    }
}
