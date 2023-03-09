using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionEffect : DamageRelatedStatusEffect, I_StatusEffect_TurnEnd, I_StatusEffect_OutgoingDamageMod
{
    public ExhaustionEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative;
        
        ApplyEffect();
    }

    public override void Perform()
    {
        Debug.LogWarning($"{pawnToEffect.Name} exahusted! {damageModifier.currentDuration}");


        damageModifier.mod *= .9f; //reduces damage mod for NEXT time
        base.Perform();
    }

}
