using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenEffect : DamageRelatedStatusEffect, I_StatusEffect_IncomingDamageMod, I_StatusEffect_TurnStart
{
    public WeakenEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        dm.currentDuration = dm.totalDuration;
        ApplyEffect();
    }

    public override void Perform()
    {
        //TEMPORARILY 1 TURN BASED

        damageModifier.currentDuration--;
        if (damageModifier.currentDuration <= 0) //in this case duration should be reduced as the statuseffect duration holds
                                                 //meaning it should have the duration of the spells effect, and reduce at the end of each turn
                                                 //hence the I_StatusEffect_TurnEnd, and why Perform() reduced duration
            EndEffect();
    }
}
