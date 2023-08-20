using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TauntedEffect : AfterActionWeightsEffect
{
    Pawn toAttack;

    float multiplier = 1.65f; // max is 2f

    //int totalDuration = 2;
    //
    
    // 99.9% this is a typo, but I must check why this guy is pawnToAvoid :O
    public TauntedEffect(Pawn pawn, Sprite sprite, Pawn pawnToAvoid, int duration) : base(pawn, sprite, duration)
    {
        alignment = EffectAlignment.Negative;
        toAttack = pawnToAvoid;
        

        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        
        base.ApplyEffect();
    }


    public override void Perform()
    {
        List<ActionVariation> toAdd = (pawnToEffect.actionPool.Where(x => x.target.Equals(toAttack.gameObject)).ToList());
        foreach (var item in toAdd)
        {
            item.weight = (int)((float)item.weight * multiplier);
        }


        base.Perform();
        //current++;
        //if (current >= totalDuration)
        //{
        //    EndEffect();
        //}
    }
}
