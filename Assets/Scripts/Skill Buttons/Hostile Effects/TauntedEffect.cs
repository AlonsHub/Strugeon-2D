using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TauntedEffect : SuggestiveEffect
{
    Pawn toAttack;

    float multiplier = 1.65f; // max is 2f

    int totalDuration = 2;
    int current;

    public TauntedEffect(Pawn pawn, Sprite sprite, Pawn pawnToAvoid) : base(pawn, sprite)
    {
        alignment = EffectAlignment.Negative;
        toAttack = pawnToAvoid;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        //AddIconToPawnBar();

        current = 0;

        base.ApplyEffect();
    }


    public override void Perform()
    {
        List<ActionVariation> toAdd = (pawnToEffect.actionPool.Where(x => x.target.Equals(toAttack.gameObject)).ToList());
        foreach (var item in toAdd)
        {
            item.weight = (int)((float)item.weight * multiplier);
        }

        current++;
        if (current >= totalDuration)
        {
            EndEffect();
        }
    }

}
