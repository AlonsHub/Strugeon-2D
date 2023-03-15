using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsightEffect : StatusEffect, I_StatusEffect_TurnEnd
{
    //Displayer prefab
    //instnace of Displayer

    //InsightResult, the may contain a target pawn, and/or an action

    //Check if ResultStillViable
    ActionVariation actionVariation;


    public InsightEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        alignment = EffectAlignment.Negative;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);

        
        actionVariation = pawnToEffect.GetIntention();

        
        

        //Add gfx 

        //Hook tests?

    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
    }

    public override void Perform()
    {
        //check one last time if the action is still do able

        //if so, delete all other actions
    }
}
