﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string playerName;

    //availavle mercs
    public List<MercName> availableMercNames;// = new List<MercName>();
    public List<MercSheet> availableMercSheet;// = new List<MercSheet>();
    public List<MercName> hireableMercs;// = new List<MercName>();

    //squads
    //public List<List<MercName>> squadsAsMercNames;
    public List<MercName> squadsAsMercNameList; // comma seperated

    public List<SquadSiteAndTimeOfDeparture> squadSitesAndTimesRemainning = new List<SquadSiteAndTimeOfDeparture>();

    /// <summary>
    /// Merc Sheets will simply hold all owned and hrieable mercs (all "existing" mercs). 
    /// Missing mercs will be generated organically, and then be added to hireables (which this list includes).
    /// By parsing these MercSheets, all information about squads(room and away), available mercs and hireables.
    /// 
    /// Meaning this needs to be done AFTER rooms are initiated!
    /// this could be done by calling to start a coroutine on start, or even on awake, which will in-turn begin with a yeild return new WaitUntil(RoomsInitiated)
    /// </summary>
    public List<MercSheet> mercSheets;// = new List<MercSheet>(); //should they be saved as squads or just have a field of RoomNumber - and if that is set to -1, that means they are available mercs. Otherwise they go the a room, which means squad? how about "away sqauds"?
                                        //POSSIBLE SOLUTION!!! - add a field of "position" - which can be a room, away-squad-number, or just available merc

    //public List<Squad> availableSquads; //holds mercs that are already gourped-up, 
    //public List<Squad> unavailableSquads; //squads that are either: on their way to a site, waiting at a site, or [some third thing yet to be imagined up]

    //public int totalSquadRooms = 1; // the total number of rooms available/occupied for/by a squad. defualt starting amount: 2
    public List<Room> rooms; // list of Merc Capacity per room 

    //gold
    public int gold;

    public int mercPrice = 20;

    public Dictionary<string, float> SiteCooldownTimes;// = new Dictionary<string, float>(); //in seconds
    public Dictionary<string, DateTime?> _siteCooldowns;// = new Dictionary<string, DateTime?>(); //in date?

    public List<string> siteNames = new List<string>(); //sitenames
    public List<string> siteCooldowns = new List<string>(); //cooldowns to string
    //site cooldowns?
    //

    public int deadMercs = 0;
    public int cowardMercs = 0;
    public int victories = 0;
    public int losses = 0;
    //public int numOfavailableMercs { get => availableMercs.Count + PartyMaster.Instance.NumOfMercsInSquads(); } //this needs to die
    public int numOfavailableMercs { get => PlayerDataMaster.Instance.GetAmountOfMercSheetsByAssignments(new List<MercAssignment> { MercAssignment.Available, MercAssignment.AwaySquad, MercAssignment.Room }); } //ACCESS LIST BY METHOD!!!!
    public int totalMercLevel { get => PlayerDataMaster.Instance.GetTotalMercLevel(); }

    public List<MagicItem> magicItems;

    
    /// Reveal ranges
    public float siteRevealIntensity;
    public float idRevealIntensity;
    public float levelRevealIntensity;

    public PsionSpectrumProfile psionSpectrum;
    public int psionProgressionLevel => psionSpectrum.SumOfAllCapacities() - 6; // reduce by [number of bars minus 1]

    public PlayerData()
    {
        mercSheets = new List<MercSheet>();

        availableMercNames = new List<MercName>();
        availableMercSheet = new List<MercSheet>();
        hireableMercs = new List<MercName>();

        SiteCooldownTimes = new Dictionary<string, float>(); //in seconds
        _siteCooldowns = new Dictionary<string, DateTime?>(); //in date?

        //siteRevealIntensity = 2;
        //idRevealIntensity = 1;
        //levelRevealIntensity = 0;
        siteRevealIntensity = 5;
        idRevealIntensity = 5;
        levelRevealIntensity = 5;
        psionSpectrum = new PsionSpectrumProfile();
    }

    void CreateAddMerc(MercName newName, MercAssignment assignment) 
    {
        MercSheet newSheet;
        switch (assignment) //other case are only used if this code is used to also load save
        {
            case MercAssignment.Null:
                break;
            case MercAssignment.AwaySquad:
                break;
            case MercAssignment.Room:
                break;
            case MercAssignment.Available:
                availableMercNames.Add(newName);
                newSheet = new MercSheet(newName, MercAssignment.Available, -1);
                mercSheets.Add(newSheet);
                break;
            case MercAssignment.Hireable:
                //availableMercs.Add(newName);
                hireableMercs.Add(newName);
                newSheet = new MercSheet(newName, MercAssignment.Hireable, -1);
                mercSheets.Add(newSheet);
                break;
            case MercAssignment.NotAvailable:
                break;
            default:
                break;
        }
        //if (availableMercs == null)
        //    availableMercs = new List<MercName>(); //Shouldn't happen seeing as that only happens with the collection version CreateAddMercs at game start AND on load it should also recieve a list
        
        // SHOULD I NOT JUST ADD THIS MERC TO THE LOG HERE?
    }
    public void CreateAddMercs(List<MercName> newNames, MercAssignment assignment) // only way to publicly add mercs is by collection 
    {
        //if (availableMercs == null) //kind of shit, but it is safe this way so... worth it
        //    availableMercs = new List<MercName>();

        foreach (var newName in newNames)
        {
            //availableMercs.Add(newName);
            //MercSheet newSheet = new MercSheet(newName);
            //mercSheets.Add(newSheet);
            CreateAddMerc(newName, assignment);
            // SHOULD I NOT JUST ADD THeSe MERCs TO THE LOG HERE?
        }
    }
    public void ChangeMercAssignment(MercName mercName, MercAssignment mercAssignment, int relevantNum)
    {
        //should switch on the assignment and re-enlist in the correct lists i.e. hireable, available etc...

        mercSheets.Where(x => x.characterName == mercName).SingleOrDefault().SetToState(mercAssignment, relevantNum);
    }

    public void RemoveMercSheet(MercName mn)
    {

        if(!mercSheets.Remove(mercSheets.Where(x => x.characterName == mn).SingleOrDefault()))
        {
            Debug.LogError("couldn't find " + mn.ToString() + " to remove");
        }
    }

    public void SaveCooldownsToLists()
    {
        siteCooldowns.Clear();
        siteNames.Clear();
        foreach (var item in _siteCooldowns.Values.ToList())
        {
            if (!item.HasValue)
                siteCooldowns.Add("nocooldown");
            else
                siteCooldowns.Add(item.ToString());
        }
        //siteCooldowns = _siteCooldowns.Values.ToList();
        siteNames = _siteCooldowns.Keys.ToList();
    }


}
