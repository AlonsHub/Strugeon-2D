using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsightEffect : SuggestiveEffect, I_StatusEffect_TurnEnd
{
   
    ActionVariation actionVariation;
    Pawn actionsTgtPawn;
    InsightDisplay insightDisplay;
    

    public InsightEffect(Pawn target, Sprite sprite, InsightDisplay iDisplay, int duration) : base(target, sprite, duration)
    {
        insightDisplay = iDisplay;
        alignment = EffectAlignment.Negative;
        ApplyEffect();
        current = totalDuration;
    }

    public override void ApplyEffect()
    {
        actionVariation = pawnToEffect.GetIntention();
        actionsTgtPawn = actionVariation.target.GetComponent<Pawn>();
        //IMPORTANT!!!
        pawnToEffect.AddStatusEffect(this);

        //Add gfx 
        PrintIntention();
        //add hook to next-turn
        TurnMachine.Instance.OnNextTurn += PrintIntention;
    }

    public override void EndEffect()
    {
        Debug.LogError("supposed unsub");
        TurnMachine.Instance.OnNextTurn -= PrintIntention;

        //Remove gfx
        insightDisplay.KillMe();
        pawnToEffect.RemoveStatusEffect(this);


    }

    public override void Perform()
    {
        //check one last time if the action is still doable
        if (pawnToEffect.ActionPoolContainsVariation(actionVariation))
        {
            //if so, delete all other actions
            pawnToEffect.actionPool.Clear();
            pawnToEffect.actionPool.Add(actionVariation);
        }
        else
        {
            Debug.LogError("whoops");
        }

        //Has not "duration" at the moment... TBD?
        current--;
        if(current <= 0)
        EndEffect();
    }

    void PrintIntention()
    {
        if (actionVariation != null)
        {
            pawnToEffect.SpeculateActionList();
            
            if (pawnToEffect.ActionPoolContainsVariation(actionVariation))
            {
                insightDisplay.SetMe(actionVariation);
                return;
            }

            Debug.LogError("Re-Roll action");
            current++;
            actionVariation = pawnToEffect.GetIntention();
            insightDisplay.SetMe(actionVariation);
        }
    }
}
