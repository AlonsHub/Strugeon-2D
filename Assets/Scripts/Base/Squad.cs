using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SquadState {InRoom, OnRoute, InPosition}; //LickingWounds, DownTime, FullScaleRevolt... interesting options
[System.Serializable]
public class Squad : IEquatable<Squad>
{
    //data group of which pawns make this group up

    //contains modified-pawns (as refs to prefabs?)
    //could instead have list of names and a DIFFERENT list for modifiers (if any)

    public List<Pawn> pawns;
    public string squadName => (pawns.Count >0)? pawns[0].Name + " Squad" : "Crew ";

    public SquadState squadState;

    public Sprite SquadPortrait => (pawns[0].PortraitSprite); //or null if none?

    public Squad()
    {
        pawns = new List<Pawn>();
    }

    public List<Sprite> IconSprites()
    {
        List<Sprite> sprites = new List<Sprite>();

        foreach (var item in pawns)
        {
            sprites.Add(item.PortraitSprite);
        }

        return sprites;
    }

    public bool isAvailable = true;

    public int roomNumber = -1;
    //public Squad(List<Pawn> newPawns)
    //{
    //    pawns = new List<Pawn>();
    //    pawns.AddRange(newPawns);

    //    //foreach (var item in pawns)
    //    //{
    //    //    item.characterSheet = PlayerDataMaster.Instance.SheetByName(item.mercName);
    //    //}

    //    squadName = pawns[0].Name + " Squad";
    //}
    public Squad(List<Pawn> newPawns, int roomNum) //weird that I don't use this... // USING IT NOW, thanks <3
    {
        pawns = new List<Pawn>();
        pawns.AddRange(newPawns); //reminder that these are just references to the prefabs, will be replaced with list of MercSheets


        SetMercsToAssignment(MercAssignment.Room, roomNum);
        //foreach (var item in pawns)
        //{
        //    item.mercSheetInPlayerData.SetToState(MercAssignment.Room, roomNum);
        //}

        //roomNumber = roomNum;
        //squadName = pawns[0].Name + " Squad";
        squadState = SquadState.InRoom;
    }
    public Squad(List<Pawn> newPawns, SquadState newState, int relevantNum) //weird that I don't use this... // USING IT NOW, thanks <3
    {
        pawns = new List<Pawn>();
        pawns.AddRange(newPawns); //reminder that these are just references to the prefabs, will be replaced with list of MercSheets

        switch (newState)
        {
            case SquadState.InRoom:
                SetMercsToAssignment(MercAssignment.Room, relevantNum);
                break;
            case SquadState.OnRoute:
                SetMercsToAssignment(MercAssignment.AwaySquad, relevantNum);

                break;
            //case SquadState.InPosition:
            //    SetMercsToAssignment(MercAssignment.AwaySquad, relevantNum);

            //    break;
            default:
                break;
        }
        //foreach (var item in pawns)
        //{
        //    item.mercSheetInPlayerData.SetToState(MercAssignment.Room, roomNum);
        //}

        //roomNumber = roomNum;
        //squadName = pawns[0].Name + " Squad";
        
        squadState = newState;
    }

    public bool AddMerc(Pawn merc) //returns false if couldn't add
    {
        if (pawns.Contains(merc))
            return false;

        pawns.Add(merc);
        merc.mercSheetInPlayerData.SetToState(MercAssignment.Room, roomNumber);
        return true;
    }
    public bool RemoveMerc(Pawn merc) //returns false if couldn't remove
    {
        if (pawns.Contains(merc))
        {
            pawns.Remove(merc);
            merc.mercSheetInPlayerData.SetToState(MercAssignment.Available, -1); //number irrelevant
            return true;
        }
        return false;
    }
    public bool RemoveMerc(MercName mercName) //returns false if couldn't remove
    {
        Pawn temp = pawns.Where(x => x.mercName == mercName).FirstOrDefault();
        if(temp)
        {
            temp.mercSheetInPlayerData.SetToState(MercAssignment.Available, -1); //number irrelevant
        }
        return pawns.Remove(temp);
    }

    public bool Equals(Squad other)
    {
        List<Pawn> tempList = new List<Pawn>(pawns);

        if (pawns.Count != other.pawns.Count)
        {
            return false;
        }
        foreach (Pawn p in other.pawns)
        {
            tempList.Remove(p);
        }
        return tempList.Count == 0;
    }

    List<MercName> PawnsAsNames()
    {
        List<MercName> toReturn = new List<MercName>();
        foreach (var item in pawns)
        {
            toReturn.Add(item.mercName);
        }

        return toReturn;
    } //depricated?

    //Could also have a SetSquadToAssignment
    public void SetMercsToAssignment(MercAssignment newAssignment, int roomNum)
    {
        switch (newAssignment)
        {
            case MercAssignment.Null:
                break;
            case MercAssignment.AwaySquad:
                
                if(!PartyMaster.Instance.squads.Remove(this)) //should be managed by the squad itself in SetMercToAssignment? //HERE IT IS!
                {
                    Debug.LogWarning("couldn't remove squad, it's OK if game is loading now, recently added squads that are OnRoute should not be PartyMaster.Sqauds so its fine"); //
                }
                PartyMaster.Instance.awaySquads.Add(this);
                isAvailable = false;
                if (this.roomNumber < 0)
                    this.roomNumber = roomNum;
                break;
            case MercAssignment.Room:
                isAvailable = true;
                this.roomNumber = roomNum;
                break;
            case MercAssignment.Available:
                break;
            case MercAssignment.Hireable:
                break;
            case MercAssignment.NotAvailable:
                break;
            default:
                break;
        }
        foreach (var item in pawns)
        {
            item.mercSheetInPlayerData.SetToState(newAssignment, roomNum);
        }
    }
}
