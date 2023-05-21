using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink_DodgeEffect : DamageRelatedStatusEffect, I_StatusEffect_IncomingDamageMod, I_StatusEffect_TurnEnd
{
    int missPercent;
    public Shrink_DodgeEffect(Pawn target, Sprite sprite, DamageModifier dm, int misschance) : base(target, sprite, dm)
    {
        missPercent = misschance;
        alignment = EffectAlignment.Positive;
        ApplyEffect();
    }

    public override float OperateOnDamage(float originalDamage)
    {
        if (Random.Range(1, 100) <= missPercent)
        {
            Debug.LogError("missed me because I'm tiny!");
            return 0f;
        }

        return base.OperateOnDamage(originalDamage);
    }

}
