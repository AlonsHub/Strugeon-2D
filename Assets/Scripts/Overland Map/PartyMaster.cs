using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMaster : MonoBehaviour
{
    
    public static PartyMaster Instance;
    public List<Pawn> currentMercParty; //Player champions as prefabs/data NOT instantiated objects themselves
    public List<Pawn> availableMercs; //Mercs you HAVE 

    public List<Squad> squads; //both available and OtW


    void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        squads = new List<Squad>();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //LATE START INSTEAD?
        LoadUpAvailableMercs();
    }

    public void LoadUpAvailableMercs() //on the loaded party
    {
        availableMercs = new List<Pawn>();
        foreach (MercName mercName in PlayerDataMaster.Instance.currentPlayerData.availableMercs)
        {
            availableMercs.Add(MercPrefabs.Instance.EnumToPawnPrefab(mercName));
        }
    }
    public void LoadUpAvailableMercs(List<MercName> mercNames)
    {
        availableMercs = new List<Pawn>();
        foreach (MercName mercName in mercNames)
        {
            availableMercs.Add(MercPrefabs.Instance.EnumToPawnPrefab(mercName));
        }
    }

    //public void SwapThisPartyIn(List<Pawn> newParty)
    //{
    //    //if(currentMercParty.Count >0)
    //    //{
    //    //    foreach (var p in currentMercParty)
    //    //    {
    //    //        a
    //    //    }
    //    //}
    //    currentMercParty.Clear();
    //    availableMercs.AddRange(currentMercParty);
    //    currentMercParty = newParty;
    //    RefMaster.Instance.mercs = currentMercParty;

    //    foreach (Pawn p in newParty)
    //    {
    //        availableMercs.Remove(p);
    //    }
    //}

    public void AddNewSquad(List<Pawn> ps)
    {
        availableMercs.RemoveAll(x => ps.Contains(x));

        squads.Add(new Squad(ps));
    }
}
