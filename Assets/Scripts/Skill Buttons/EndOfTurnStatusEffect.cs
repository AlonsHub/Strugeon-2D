using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTurnStatusEffect : StatusEffect
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
