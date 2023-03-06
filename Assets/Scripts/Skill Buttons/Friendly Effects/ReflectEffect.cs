using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectEffect : StatusEffect, I_StatusEffect_TurnEnd
{

    int totalDuration = 2;
    int current;

    public ReflectEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        alignment = EffectAlignment.Positive;

        current = totalDuration;
        ApplyEffect();
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
        current--;
        if (current <= 0)
        {
            EndEffect();
        }
    }
}
