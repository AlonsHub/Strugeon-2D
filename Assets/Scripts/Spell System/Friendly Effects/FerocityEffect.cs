using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerocityEffect : DamageRelatedStatusEffect, I_StatusEffect_TurnEnd ,I_StatusEffect_IncomingDamageMod
{
    public FerocityEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Positive; //See comments on "Buff Bundles" in BlindRageEffect 

        ApplyEffect();
    }

    public override float OperateOnDamage(float originalDamage)
    {
        if ((pawnToEffect.currentHP - originalDamage) < 1)
            return pawnToEffect.currentHP - 1;
        else
            return originalDamage;
    }
}
