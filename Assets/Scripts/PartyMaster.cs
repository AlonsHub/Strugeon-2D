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
        }
        Instance = this;

        DontDestroyOnLoad(this);
    }


    public void SwapThisPartyIn(List<Pawn> newParty)
    {
        availableMercs.AddRange(currentMercParty);
        currentMercParty = newParty;
        foreach (Pawn p in newParty)
        {
            availableMercs.Remove(p);
        }
    }
}
