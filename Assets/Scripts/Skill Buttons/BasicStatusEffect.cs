using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStatusEffect : StatusEffect
{
    public BasicStatusEffect(Pawn target, Sprite sprite) : base(target, sprite)
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
