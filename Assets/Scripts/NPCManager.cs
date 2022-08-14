using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private bool _isInteracting = false;
    private NPC _activeNPC;
    private List<NPC> nPCs = new List<NPC>();
    public Sprite[] guideSprites;
    public NPC ActiveNPC
    {
        get { return _activeNPC; }
        set { _activeNPC = value; }
    }

    private void Update()
    {
        if(_activeNPC == null)
            return;

        if(GameManager.Instance.player.CanInteract && !_isInteracting && _activeNPC != null && Input.GetKeyDown(KeyCode.I) && !GameManager.Instance.IsBlockGameActions)
        {
            _isInteracting = _activeNPC.TryInteract();
        }

        if(_isInteracting && !GameManager.Instance.IsBlockGameActions)
        {
            if(_activeNPC.isShopNPC)
                GameManager.Instance.ShowShop(_activeNPC.nPCType);
            else
                ResetActiveNPC();
        }

    }

    public void AddNPC(NPC nPCToAdd)
    {
        int index = nPCs.FindIndex(x => x.nPCID == nPCToAdd.nPCID);
        if(index >= 0)
            nPCs.RemoveAt(index);
        nPCs.Add(nPCToAdd);
    }
    
    public string GetNPCID(Common.NPCType nPCType)
    {
        NPC nPC = nPCs.FirstOrDefault(x => x.nPCType == nPCType);

        if(nPC != null)
            return nPC.nPCID;

        return null;
    }

    public string GetNPCName(string nPCID)
    {
        NPC nPC = nPCs.FirstOrDefault(x => x.nPCID == nPCID);

        if(nPC != null)
            return nPC.nPCName;

        return null;
    }

    public string[] GetNPCDialogs(string nPCID)
    {
        NPC nPC = nPCs.FirstOrDefault(x => x.nPCID == nPCID);

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

    public void ResetActiveNPC()
    {
        _isInteracting = false;
        if(_activeNPC != null)
        {
            _activeNPC.ResetInteractability();
            _activeNPC = null;
        }   
    }

    public void ResetNPCList()
    {
        nPCs.Clear();
    }
}
