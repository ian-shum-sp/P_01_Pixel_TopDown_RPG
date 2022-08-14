using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningDialogTrigger : Collidable
{
    private bool _isTriggered = false;
    public Common.NPCType npcToTrigger;

    protected override void OnCollide(Collider2D collider)
    {
        if(_isTriggered)
            return;

        if(collider.name == "Player")
        {
            _isTriggered = true;
            string nPCID = GameManager.Instance.GetNPCID(npcToTrigger);
            GameManager.Instance.ShowRunningDialog(nPCID, false);
        }
    }   

}
