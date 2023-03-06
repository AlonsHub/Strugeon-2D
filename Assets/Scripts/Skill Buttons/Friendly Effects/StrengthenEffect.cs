using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthenEffect : DamageRelatedStatusEffect, I_StatusEffect_OutgoingDamageMod, I_StatusEffect_TurnEnd //This interface's logic already implemented in parent
{
    public StrengthenEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Positive;

        dm.currentDuration = dm.totalDuration;
        ApplyEffect();
    }

    /// <summary>
    /// MUST PERFORM THIS BASE!
    /// IF OVERRIDE THIS, CALL BASE TO ADD STATUS EFFECT TO PAWN!
    /// </summary>
    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);
    }
    /// <summary>
    /// MUST PERFORM THIS BASE!
    /// </summary>
    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
    }

    public override void Perform()
    {
        if (damageModifier.currentDuration <= 0) //in this case, duration is reduced where damage is rolled and where it is modified, in WeaponItem (so it does NOT reduce if not used)
            EndEffect();
    }
}
