using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionEffect : DamageRelatedStatusEffect, I_StatusEffect_TurnEnd, I_StatusEffect_OutgoingDamageMod
{
    public ExhaustionEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative;
        //dm.mod = .9f; 
        dm.currentDuration = dm.totalDuration;
        ApplyEffect();
    }

    public override void Perform()
    {
        damageModifier.mod *= .9f; //reduces damage mod for NEXT time
        damageModifier.currentDuration--;
        if (damageModifier.currentDuration <= 0)
        {
            EndEffect();
        }
    }

    public override float OperateOnDamage(float originalDamage)
    {
        return damageModifier.Operate(originalDamage);
    }
}
