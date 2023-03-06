using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDamageReduction : DamageRelatedStatusEffect, I_StatusEffect_TurnStart, I_StatusEffect_IncomingDamageMod
{
    /// <summary>
    /// The sprite is left empty since this is an add-on effect to freeze which already has an icon
    /// </summary>
    /// <param name="target"></param>
    /// <param name="sprite"></param>
    /// <param name="dm"></param>
    public FreezeDamageReduction(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative; //See comments on "Buff Bundles" in BlindRageEffect 

        dm.currentDuration = dm.totalDuration;
        ApplyEffect();
    }

    public override void Perform()
    {
        damageModifier.currentDuration--;
        if (damageModifier.currentDuration <= 0) //in this case duration should be reduced as the statuseffect duration holds
                                                 //meaning it should have the duration of the spells effect, and reduce at the end of each turn
                                                 //hence the I_StatusEffect_TurnEnd, and why Perform() reduced duration
            EndEffect();
    }
}
