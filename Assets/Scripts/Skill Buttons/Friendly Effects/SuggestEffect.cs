using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SuggestEffect : AfterActionWeightsEffect
{
    float mod;
    GameObject target;
    public SuggestEffect(Pawn pawn, GameObject target, Sprite sprite, int duration, float modifier) : base(pawn, sprite, duration)
    {
        mod = modifier;
        alignment = EffectAlignment.Positive;
        ApplyEffect();
    }

    public override void Perform()
    {
        ActionVariation[] variations = pawnToEffect.GetActionVariationsByPredicate(x => x.target == target || x.secondaryTarget == target);

        if(variations == null || variations.Length ==0)
            return;
        
        foreach (var item in variations)
        {
            item.weight =(int)(item.weight*mod);
        }
        current--;
        if (current <= 0)
            EndEffect();
    }
}
