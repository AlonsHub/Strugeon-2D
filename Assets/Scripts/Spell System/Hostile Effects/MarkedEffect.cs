using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedEffect : DamageRelatedStatusEffect, I_StatusEffect_IncomingDamageMod, I_StatusEffect_TurnEnd
{
    public MarkedEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative;

        ApplyEffect();
    }

}
