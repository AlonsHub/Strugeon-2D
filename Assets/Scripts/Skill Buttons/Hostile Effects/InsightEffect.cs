using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsightEffect : SuggestiveEffect, I_StatusEffect_TurnEnd
{
    //Displayer prefab
    //instnace of Displayer

    //InsightResult, the may contain a target pawn, and/or an action

    //Check if ResultStillViable
    ActionVariation actionVariation;
    Pawn tgtPawn;



    public InsightEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        alignment = EffectAlignment.Negative;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        actionVariation = pawnToEffect.GetIntention();
        tgtPawn = actionVariation.target.GetComponent<Pawn>();
        //IMPORTANT!!!
        pawnToEffect.AddStatusEffect(this);

        //Add gfx 

        //add hook to next-turn
        TurnMachine.Instance.OnNextTurn += PrintIntention;
    }

    public override void EndEffect()
    {
        Debug.LogError("supposed unsub");
        TurnMachine.Instance.OnNextTurn -= PrintIntention;
        pawnToEffect.RemoveStatusEffect(this);

        //Remove gfx


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
        EndEffect();
    }

    void PrintIntention()
    {
        if(actionVariation != null)
        {
            if(tgtPawn)
            Debug.LogError($"{tgtPawn.Name} with {actionVariation.relevantItem}");
            else
            Debug.LogError($"{actionVariation.target} with {actionVariation.relevantItem}");
                
        }
    }
}
