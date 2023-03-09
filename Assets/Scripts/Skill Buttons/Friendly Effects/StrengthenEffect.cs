using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthenEffect : DamageRelatedStatusEffect, I_StatusEffect_OutgoingDamageMod, I_StatusEffect_TurnEnd //This interface's logic already implemented in parent
{
    public StrengthenEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Positive;

        ApplyEffect();
    }
}
