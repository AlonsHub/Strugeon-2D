using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad 
{
    //data group of which pawns make this group up

    //contains modified-pawns (as refs to prefabs?)
    //could instead have list of names and a DIFFERENT list for modifiers (if any)
    
    public List<Pawn> pawns;

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

}
