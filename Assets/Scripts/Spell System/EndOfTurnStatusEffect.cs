using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//weird now that we use the interface...
public class EndOfTurnStatusEffect : StatusEffect, I_StatusEffect_TurnEnd
{
    public EndOfTurnStatusEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        //Dont apply effect on all - just in case you want a clean one
    }

    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);
    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
    }

    public override void Perform()
    {
        
    }

   
}
