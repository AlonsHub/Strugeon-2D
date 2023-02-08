using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalmEffect : SuggestiveEffect
{
    
    int totalDuration = 2;
    int current;

    public CalmEffect(Pawn pawn, Sprite s) : base(pawn, s)
    {
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        current = 0;
        pawnToEffect.AddSuggestiveEffect(this);
        pawnToEffect.AddEffectIcon(iconSprite, "calmDebuff");
    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveSuggestiveEffect(this);
        pawnToEffect.RemoveIconByName("calmDebuff");
    }

    public override void Perform()
    {
        List<ActionVariation> toRemove= (pawnToEffect.actionPool.Where(x => x.relevantItem is SA_Item).ToList());
        foreach (var item in toRemove)
        {
            pawnToEffect.actionPool.Remove(item);
        }
        current++;
        if(current >= totalDuration)
        {
            EndEffect();
        }
    }

    
}
