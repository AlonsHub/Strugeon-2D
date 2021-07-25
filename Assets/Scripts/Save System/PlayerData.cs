using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string playerName;

    //availavle mercs
    public List<MercName> availableMercs;

    //squads?
    public List<Squad> availableSquads; //holds mercs that are already gourped-up, 
    public List<Squad> unavailableSquads; //squads that are either: on their way to a site, waiting at a site, or [some third thing yet to be imagined up]

    //gold
    public int gold;

    //site cooldowns?
    //
}
