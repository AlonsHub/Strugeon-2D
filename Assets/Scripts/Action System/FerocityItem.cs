using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerocityItem : ActionItem
{
    public override void Action(GameObject tgt)
    {
        //this case has no target/s as far as I understand it, it affects just the merc using it
    }

    public override void CalculateVariations()
    {
        if(pawn.currentHP >= pawn.maxHP/10f)
        {
            return;
        }

        
    }
}
