using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Squad 
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
        squadName = pawns[0].Name + " Squad";
    }
    public Squad(List<Pawn> newPawns, int roomNum) //weird that I don't use this...
    {
        pawns = new List<Pawn>();
        pawns.AddRange(newPawns);
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
}
