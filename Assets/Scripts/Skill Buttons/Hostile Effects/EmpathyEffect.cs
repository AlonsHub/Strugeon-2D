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

        pawnToEffect.AddSuggestiveEffect(this);
        pawnToEffect.AddEffectIcon(iconSprite, "empathyDebuff");
    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveIconByName("empathyDebuff");
        pawnToEffect.RemoveSuggestiveEffect(this);
    }

    public override void Perform()
    {
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
