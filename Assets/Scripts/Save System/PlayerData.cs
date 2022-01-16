using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string playerName;

    //availavle mercs
    public List<MercName> availableMercs = new List<MercName>();
    public List<MercName> hireableMercs;

    //squads
    //public List<List<MercName>> squadsAsMercNames;
    public List<MercName> squadsAsMercNameList; // comma seperated
    
    /// <summary>
    /// Merc Sheets will simply hold all owned and hrieable mercs (all "existing" mercs). 
    /// Missing mercs will be generated organically, and then be added to hireables (which this list includes).
    /// By parsing these MercSheets, all information about squads(room and away), available mercs and hireables.
    /// 
    /// Meaning this needs to be done AFTER rooms are initiated!
    /// this could be done by calling to start a coroutine on start, or even on awake, which will in-turn begin with a yeild return new WaitUntil(RoomsInitiated)
    /// </summary>
    public List<MercSheet> mercSheets = new List<MercSheet>(); //should they be saved as squads or just have a field of RoomNumber - and if that is set to -1, that means they are available mercs. Otherwise they go the a room, which means squad? how about "away sqauds"?
                                        //POSSIBLE SOLUTION!!! - add a field of "position" - which can be a room, away-squad-number, or just available merc

    //public List<Squad> availableSquads; //holds mercs that are already gourped-up, 
    //public List<Squad> unavailableSquads; //squads that are either: on their way to a site, waiting at a site, or [some third thing yet to be imagined up]

    //public int totalSquadRooms = 1; // the total number of rooms available/occupied for/by a squad. defualt starting amount: 2
    public List<Room> rooms; // list of Merc Capacity per room 

    //gold
    public int gold;

    public int mercPrice = 20;

    public Dictionary<string, float> SiteCooldownTimes = new Dictionary<string, float>(); //in seconds

    public List<string> siteNames; //sitenames
    public List<float> siteCooldowns; //cooldowns
    //site cooldowns?
    //

    public int deadMercs = 0;
    public int cowardMercs = 0;
    public int victories = 0;
    public int losses = 0;
    public int numOfavailableMercs { get => availableMercs.Count + PartyMaster.Instance.NumOfMercsInSquads(); } //this needs to die

    void CreateAddMerc(MercName newName) 
    {
        //if (availableMercs == null)
        //    availableMercs = new List<MercName>(); //Shouldn't happen seeing as that only happens with the collection version CreateAddMercs at game start AND on load it should also recieve a list
        availableMercs.Add(newName);
        MercSheet newSheet = new MercSheet(newName);
        mercSheets.Add(newSheet);
        // SHOULD I NOT JUST ADD THIS MERC TO THE LOG HERE?
    }
    public void CreateAddMercs(List<MercName> newNames) // only way to publicly add mercs is by collection 
    {
        //if (availableMercs == null) //kind of shit, but it is safe this way so... worth it
        //    availableMercs = new List<MercName>();

        foreach (var newName in newNames)
        {
            //availableMercs.Add(newName);
            //MercSheet newSheet = new MercSheet(newName);
            //mercSheets.Add(newSheet);
            CreateAddMerc(newName);
            // SHOULD I NOT JUST ADD THeSe MERCs TO THE LOG HERE?
        }
    }



}
