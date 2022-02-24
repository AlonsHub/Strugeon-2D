using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Squad : IEquatable<Squad>
{
    //data group of which pawns make this group up

    //contains modified-pawns (as refs to prefabs?)
    //could instead have list of names and a DIFFERENT list for modifiers (if any)

    public List<Pawn> pawns;
    public string squadName;

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
    public Squad(List<Pawn> newPawns)
    {
        pawns = new List<Pawn>();
        pawns.AddRange(newPawns);

        //foreach (var item in pawns)
        //{
        //    item.characterSheet = PlayerDataMaster.Instance.SheetByName(item.mercName);
        //}

        squadName = pawns[0].Name + " Squad";
    }
    public Squad(List<Pawn> newPawns, int roomNum) //weird that I don't use this... // USING IT NOW, thanks <3
    {
        pawns = new List<Pawn>();
        pawns.AddRange(newPawns); //reminder that these are just references to the prefabs, will be replaced with list of MercSheets

        foreach (var item in pawns)
        {
            item.mercSheetInPlayerData.SetToState(MercAssignment.Room, roomNum);
        }

        roomNumber = roomNum;
        squadName = pawns[0].Name + " Squad";
    }

    public bool AddMerc(Pawn merc) //returns false if couldn't add
    {
        if (pawns.Contains(merc))
            return false;

        pawns.Add(merc);
        return true;
    }
    public bool RemoveMerc(Pawn merc) //returns false if couldn't remove
    {
        if (pawns.Contains(merc))
        {
            pawns.Remove(merc);

            return true;
        }
        return false;
    }
    public bool RemoveMerc(MercName mercName) //returns false if couldn't remove
    {
        return pawns.Remove(pawns.Where(x => x.mercName == mercName).FirstOrDefault());
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

    public void SetMercsToAssignment(MercAssignment newAssignment, int relevantNum)
    {
        switch (newAssignment)
        {
            case MercAssignment.Null:
                break;
            case MercAssignment.AwaySquad:
                PartyMaster.Instance.squads.Remove(this); //should be managed by the squad itself in SetMercToAssignment? //HERE IT IS!
                isAvailable = false;
                break;
            case MercAssignment.Room:
                isAvailable = true;
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
            item.mercSheetInPlayerData.SetToState(newAssignment, relevantNum);
        }
    }
}
