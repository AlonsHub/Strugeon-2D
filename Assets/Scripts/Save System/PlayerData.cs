using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string playerName;

    //availavle mercs
    public List<MercName> availableMercs;

    //squads
    public List<List<MercName>> squadsAsMercNames;
    public List<MercName> squadsAsMercNameList; // comma seperated

    //public List<Squad> availableSquads; //holds mercs that are already gourped-up, 
    //public List<Squad> unavailableSquads; //squads that are either: on their way to a site, waiting at a site, or [some third thing yet to be imagined up]

    public int totalSquadRooms = 1; // the total number of rooms available/occupied for/by a squad. defualt starting amount: 2
    public List<int> squadRoomSizes; // list of Merc Capacity per room

    //gold
    public int gold;

    //site cooldowns?
    //
}
