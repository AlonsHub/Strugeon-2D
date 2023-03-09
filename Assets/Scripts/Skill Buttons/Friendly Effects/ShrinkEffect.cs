using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkEffect : DamageRelatedStatusEffect, I_StatusEffect_OutgoingDamageMod, I_StatusEffect_TurnEnd
{
    public ShrinkEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Positive;
        ApplyEffect();
    }

    // Add gfx shrink logic to ApplyEffect() and remove on EndEffect()

    public override void ApplyEffect()
    {
        pawnToEffect.SetGFXScale(new Vector3(.5f, .5f, 1f));

        base.ApplyEffect();
    }

    public override void EndEffect()
    {
        pawnToEffect.SetGFXScale(new Vector3(1f, 1f, 1f));

        base.EndEffect();
    }


}
