using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyMaster : MonoBehaviour
{
    
    public static PartyMaster Instance;
    //public List<Pawn> currentMercParty; //Player champions as prefabs/data NOT instantiated objects themselves

    public List<Pawn> currentMercParty { get => currentSquad.pawns; }
    public Squad currentSquad;

    public List<Pawn> availableMercPrefabs; //Mercs you HAVE 

    public List<Squad> squads; //both available and OtW
    public List<Squad> awaySquads; //OtW
    
    //public List<MercName> AllMercs() //including hireables // KILL THIS WITH FIREEEE
    //{
    //    List<MercName> toReturn = new List<MercName>();

    //    foreach (var item in availableMercPrefabs)
    //    {
    //        toReturn.Add(item.mercName);
    //    }
    //    foreach (var squad in squads)
    //    {
    //        foreach (var x in squad.pawns)
    //        {
    //            toReturn.Add(x.mercName);
    //        }
    //    }
    //    if(PlayerDataMaster.Instance.currentPlayerData.hireableMercs != null && PlayerDataMaster.Instance.currentPlayerData.hireableMercs.Count >0)
    //    {
    //        foreach (var item in PlayerDataMaster.Instance.currentPlayerData.hireableMercs)
    //        {
    //            toReturn.Add(item);
    //        }
    //    }

    //    return toReturn;
    //}
    public int NumOfMercsInSquads() 
    {
        int toReturn = 0;



        foreach (var squad in squads)
        {
            toReturn += squad.pawns.Count;
            //foreach (var x in squad.pawns)
            //{
            //    toReturn++;
            //}
        }
        return toReturn;
    }

    void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        squads = new List<Squad>();
        awaySquads = new List<Squad>();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //LATE START INSTEAD?
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        LoadUpAvailableMercs();
    }

    //START Save System FIX HERE 16/01/22
    public void LoadUpAvailableMercs() //on the loaded party
    {
        availableMercPrefabs = new List<Pawn>();
        foreach (MercName mercName in PlayerDataMaster.Instance.currentPlayerData.availableMercNames)
        {
            availableMercPrefabs.Add(MercPrefabs.Instance.EnumToPawnPrefab(mercName));
        }


        //squads = ParseSquads();
        ParseSquads();

        if (squads == null)
            squads = new List<Squad>();

        
        List<Squad> toRemove = squads.Where(x => x.pawns.Count == 0).ToList();
        foreach (var s in toRemove)
        {
            squads.Remove(s);
        }



        //check that all mercSheet mercs exist in available and 

        //loadup hireable mercs? //not here, and honestly, we don't need to do this since we only need the MercNames anyway


    }
   

    void ParseSquads()
    {
        List<Squad> toSquads = new List<Squad>();
        List<Squad> toAwaySquads = new List<Squad>();
        if (PlayerDataMaster.Instance.currentPlayerData.squadsAsMercNameList == null)
            return;

        if (PlayerDataMaster.Instance.currentPlayerData.squadsAsMercNameList.Count == 0)
            return;

        int c = PlayerDataMaster.Instance.currentPlayerData.squadsAsMercNameList.Where(y => y == MercName.None).Count();
        int x = -1;
        List<MercName>[] newList = new List<MercName>[c];
        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.squadsAsMercNameList.Count; i++)
        {
            if(PlayerDataMaster.Instance.currentPlayerData.squadsAsMercNameList[i] == MercName.None)
            {
                x++;
                newList[x] = new List<MercName>();
                continue;
            }
            newList[x].Add(PlayerDataMaster.Instance.currentPlayerData.squadsAsMercNameList[i]);
        }
        int ind = 0;
        foreach(List<MercName> mercNames in newList)
        {
            
            if(mercNames.Count> 0) //???? //these question marks are actually on-to something. An empty room will regiester as two following zeros 
            {
                List<Pawn> tempPawns = PawnsFromNames(mercNames);
                Squad s = null;
                switch (tempPawns[0].mercSheetInPlayerData.currentAssignment) //checks the relevant mercSheet and the empty merc sheet on prefab
                {
                    //case MercAssignment.Null:
                    //    break;
                    case MercAssignment.AwaySquad:
                        s = new Squad(tempPawns, SquadState.OnRoute, ind);
                        toAwaySquads.Add(s); //for some reason im adding them with the roomCount, but that overload doesnt use it...
                        break;
                    case MercAssignment.Room:
                        s = new Squad(tempPawns, SquadState.InRoom, ind);
                        toSquads.Add(s); //for some reason im adding them with the roomCount, but that overload doesnt use it...
                        break;
                    case MercAssignment.Available:
                        break;
                    //case MercAssignment.Hireable:
                    //    break;
                    //case MercAssignment.NotAvailable:
                    //    break;
                    default:
                        break;
                }


                //Squad s = new Squad(tempPawns, ind);
                //    toSquads.Add(s); //for some reason im adding them with the roomCount, but that overload doesnt use it...
                if (s != null)
                PlayerDataMaster.Instance.currentPlayerData.rooms[ind].squad = s;
            }
            
            // ADD s
            ind++;
        }
        //return toSquads;
        squads = toSquads;
        awaySquads = toAwaySquads;


    }

    List<Pawn> PawnsFromNames(List<MercName> names) /// CHANGE TBD OVER HERE - these refs to pawn prefabs should be mercsheets (that CAN get refs to their prefabs, via paramaters/method that calls MercPerfabs
    {
        List<Pawn> toReturn = new List<Pawn>();

        foreach (MercName mercName in names)
        {
            toReturn.Add(MercPrefabs.Instance.EnumToPawnPrefab(mercName));
        }

        return toReturn;
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

    //public void AddNewSquad(List<Pawn> ps)
    //{
    //    availableMercs.RemoveAll(x => ps.Contains(x));

    //    squads.Add(new Squad(ps));
    //    foreach(Room r in PlayerDataMaster.Instance.currentPlayerData.rooms)
    //    {
    //        if (r.isOccupied)
    //            continue;

    //        r.isOccupied = true;
    //        r.squad = squads[squads.Count - 1];
    //        return;
    //    }
    //}

    public void AddNewSquadToRoom(List<Pawn> ps, Room r)
    {
        availableMercPrefabs.RemoveAll(x => ps.Contains(x));
        //Squad s = new Squad(ps);
        Squad s = new Squad(ps, r.roomNumber);

        //r.squad = s;
        PlayerDataMaster.Instance.currentPlayerData.rooms[r.roomNumber].squad = s;
        PlayerDataMaster.Instance.currentPlayerData.rooms[r.roomNumber].isOccupied = true;
        squads.Add(s);
        s.roomNumber = r.roomNumber;
        
    }

    //public void DisbandSquad(int i)
    //{
    //    //squads[i].
    //}
    //public void DisbandSquad(Squad s)
    //{

    //}
}
