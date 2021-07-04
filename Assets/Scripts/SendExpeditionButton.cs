using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendExpeditionButton : MonoBehaviour
{
    //[SerializeField]
    //LevelSO myLevelSO;
    [SerializeField]
    LevelEnum thisLevel;


    [SerializeField]
    ArrivalPanel ap;

    //private void Start()
    //{
    //    myLevelSO = GetComponentInParent<SiteButton>().levelSO;
    //}

    public void SendExpeditionToMySite()
    {
        //if(myLevelSO)
        //{
        //    ArenaLauncher.Instance.LoadArena(myLevelSO);
        //}
        PartyMaster.Instance.currentMercParty = ap.mercs;
        PartyMaster.Instance.availableMercs.RemoveAll(x => ap.mercs.Contains(x));

        LevelRef.Instance.SetCurrentLevel((int)thisLevel);
        SceneManager.LoadScene("ArenaSceneGeneric");
    }
    
}
