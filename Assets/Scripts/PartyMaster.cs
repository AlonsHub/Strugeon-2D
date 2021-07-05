using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMaster : MonoBehaviour
{
    
    public static PartyMaster Instance;
    public List<Pawn> currentMercParty; //Player champions as prefabs/data NOT instantiated objects themselves
    public List<Pawn> availableMercs; //Mercs you HAVE 
    void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this);
    }


    public void SwapThisPartyIn(List<Pawn> newParty)
    {
        //if(currentMercParty.Count >0)
        //{
        //    foreach (var p in currentMercParty)
        //    {
        //        a
        //    }
        //}
        currentMercParty.Clear();
        availableMercs.AddRange(currentMercParty);
        currentMercParty = newParty;
        RefMaster.Instance.mercs = currentMercParty;

        foreach (Pawn p in newParty)
        {
            availableMercs.Remove(p);
        }
    }
}
