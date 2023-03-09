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

        ApplyEffect();
    }

}
