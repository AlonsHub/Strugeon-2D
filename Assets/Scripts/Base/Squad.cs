using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Squad 
{
    //data group of which pawns make this group up

    //contains modified-pawns (as refs to prefabs?)
    //could instead have list of names and a DIFFERENT list for modifiers (if any)
    
    public List<Pawn> pawns;
    public Squad()
    {
        pawns = new List<Pawn>();
    }
    public Squad(List<Pawn> newPawns)
    {
        pawns = new List<Pawn>();
        pawns.AddRange(newPawns);
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
}
