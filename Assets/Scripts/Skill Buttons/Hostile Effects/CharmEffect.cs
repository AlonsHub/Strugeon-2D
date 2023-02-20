using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmEffect : StatusEffect
{
    public CharmEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        
    }

    public override void EndEffect()
    {
        
    }

    public override void Perform()
    {
        throw new System.NotImplementedException();
    }
}
