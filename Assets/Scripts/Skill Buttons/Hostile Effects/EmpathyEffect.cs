using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmpathyEffect : SuggestiveEffect
{
    Pawn toAvoid;

    int totalDuration = 2;
    int current;

    public EmpathyEffect(Pawn pawn, Sprite sprite, Pawn pawnToAvoid) : base(pawn, sprite)
    {
        toAvoid = pawnToAvoid;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        current = 0;

        base.ApplyEffect();
    }


    public override void Perform()
    {
        if (toAvoid == null)
        {
            EndEffect();
            return;
        }

        List<ActionVariation> toRemove = (pawnToEffect.actionPool.Where(x => x.target.Equals(toAvoid.gameObject)).ToList());
        foreach (var item in toRemove)
        {
            pawnToEffect.actionPool.Remove(item);
        }
        current++;
        if (current >= totalDuration)
        {
            EndEffect();
        }
    }
}
