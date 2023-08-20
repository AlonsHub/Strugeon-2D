﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalmEffect : AfterActionWeightsEffect
{
    
    public CalmEffect(Pawn pawn, Sprite s, int duration) : base(pawn, s, duration)
    {
        alignment = EffectAlignment.Negative;
        ApplyEffect();
    }

    //public override void ApplyEffect()
    //{
    //    //current = 0;
    //    //pawnToEffect.AddSuggestiveEffect(this);
    //    base.ApplyEffect();
    //}

    public override void Perform()
    {
        List<ActionVariation> toRemove= (pawnToEffect.actionPool.Where(x => x.relevantItem is SA_Item).ToList());
        foreach (var item in toRemove)
        {
            pawnToEffect.actionPool.Remove(item);
        }
        //current--;
        //if(current <= 0)
        //{
        //    EndEffect();
        //}
        base.Perform();
    }

    public override void StackMe()
    {
        current = totalDuration;
    }
}
