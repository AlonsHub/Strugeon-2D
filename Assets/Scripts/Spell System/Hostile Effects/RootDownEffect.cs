using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDownEffect : StatusEffect, I_StatusEffect_TurnStart
{
    float spikeDamage;

    int currentDuration;
    int totalDuration;

    RootDownAttacher rootDownAttacher;

    public RootDownEffect(Pawn target, Sprite sprite, RootDownAttacher attacher) : base(target, sprite)
    {
        alignment = EffectAlignment.Negative;

        spikeDamage = attacher.spikeDamage;

        totalDuration = attacher.maxTTL;
        currentDuration = attacher.maxTTL;

        rootDownAttacher = attacher;

        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);
    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
        rootDownAttacher.RemoveEffect();
    }

    public override void Perform()
    {
        rootDownAttacher.DamageHost((int)spikeDamage);
        currentDuration--;
        if(currentDuration <= 0)
        {
            EndEffect();
        }
    }
    public override void StackMe()
    {
        currentDuration = totalDuration;
    }
}
