using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistenceEffect : DamageRelatedStatusEffect, I_StatusEffect_TurnStart, I_StatusEffect_IncomingDamageMod
{
    public ResistenceEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative; //See comments on "Buff Bundles" in BlindRageEffect 

        ApplyEffect();
    }

    //public override void Perform()
    //{
    //    damageModifier.currentDuration--;
    //    if (damageModifier.currentDuration <= 0)                       
    //        EndEffect();
    //}

}
