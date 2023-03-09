using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenEffect : DamageRelatedStatusEffect, I_StatusEffect_IncomingDamageMod, I_StatusEffect_TurnStart
{
    public WeakenEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative;
        ApplyEffect();
    }

    public override float OperateOnDamage(float originalDamage)
    {

        return base.OperateOnDamage(originalDamage);
    }
}
