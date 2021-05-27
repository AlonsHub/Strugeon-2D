using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ExpeditionButton : MonoBehaviour
{
    public LevelSO levelSO;
    public List<Pawn> partyPawns;
    public void LoadArenaScene()
    {
        PartyMaster.Instance.SwapThisPartyIn(partyPawns);
        TransferSceneToArena.levelSO = levelSO; //KWWWWAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        SceneManager.LoadScene(1);
    }
    //public void SetUpButton(LevelSO lvlSo, List<Pawn> newParty)
    //{
    //    levelSO = lvlSo;
    //    partyPawns = newParty;
    //}
}
