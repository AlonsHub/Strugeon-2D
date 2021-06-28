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
        LevelRef.Instance.SetCurrentLevel((int)thisLevel);
        SceneManager.LoadScene("ArenaSceneGeneric");
    }
    
}
