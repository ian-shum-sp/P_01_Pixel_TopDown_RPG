using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private bool _isInteracted = false;
    private NPC _interactedNPC = null;
    private List<NPC> nPCs = new List<NPC>();
    public Sprite[] guideSprites;

    private void Update()
    {
        if(!_isInteracted)
        {
            foreach(NPC nPC in nPCs)
            {
                if(Input.GetKeyDown(KeyCode.I))
                {
                    _isInteracted = nPC.TryInteract();
                    _interactedNPC = nPC;
                }

                nPC.ResetInteractability();

                if(_isInteracted == true)
                    break;
            }
        }   

        if(_isInteracted && !GameManager.Instance.IsBlockGameActions)
        {
            if(_interactedNPC.isShopNPC)
                GameManager.Instance.ShowShop(_interactedNPC.nPCType);
                
            _isInteracted = false;
            _interactedNPC = null;
        }
       
    }

    public void AddNPC(NPC nPCToAdd)
    {
        int index = nPCs.FindIndex(x => x.nPCType == nPCToAdd.nPCType);
        if(index >= 0)
            nPCs.RemoveAt(index);
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
