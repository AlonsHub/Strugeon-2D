using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindRageExtraDamageEffect : DamageRelatedStatusEffect, I_StatusEffect_TurnEnd, I_StatusEffect_OutgoingDamageMod
{
    public BlindRageExtraDamageEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, null, dm) //pass null instead of sprite just to ensure it won't add a status icon by accident
    {
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
