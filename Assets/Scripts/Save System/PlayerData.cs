using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string playerName;

    //availavle mercs
    public List<MercName> availableMercs;
    public List<MercName> hireableMercs;

    //squads
    //public List<List<MercName>> squadsAsMercNames;
    public List<MercName> squadsAsMercNameList; // comma seperated

    //public List<Squad> availableSquads; //holds mercs that are already gourped-up, 
    //public List<Squad> unavailableSquads; //squads that are either: on their way to a site, waiting at a site, or [some third thing yet to be imagined up]

    //public int totalSquadRooms = 1; // the total number of rooms available/occupied for/by a squad. defualt starting amount: 2
    public List<Room> rooms; // list of Merc Capacity per room

    //gold
    public int gold;

    public int mercPrice = 20;

    public Dictionary<string, float> SiteCooldownTimes = new Dictionary<string, float>(); //in seconds

    public List<string> keys; //sitenames
    public List<float> values; //cooldowns
    //site cooldowns?
    //

    public int deadMercs = 0;
    public int cowardMercs = 0;
    public int victories = 0;
    public int losses = 0;
    public int numOfavailableMercs { get => availableMercs.Count+PartyMaster.Instance.NumOfMercsInSquads(); }
    //public int numOfavailableMercs { get => availableMercs.Count ; }
}
