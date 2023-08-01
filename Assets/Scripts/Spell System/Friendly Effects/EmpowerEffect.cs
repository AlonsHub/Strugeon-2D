using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpowerEffect : DamageRelatedStatusEffect, I_StatusEffect_OutgoingDamageMod, I_StatusEffect_TurnEnd //This interface's logic already implemented in parent
{
    public EmpowerEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Positive;
        //dm.currentDuration = -100; //Just don't call base perform
        ApplyEffect();
    }

    public override void Perform()
    {
        //base.Perform();
        //Do nothing basically
    }
}
