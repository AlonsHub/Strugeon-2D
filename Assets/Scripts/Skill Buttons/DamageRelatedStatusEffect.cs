using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRelatedStatusEffect : StatusEffect
{
    protected DamageModifier damageModifier;

    public DamageRelatedStatusEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite)
    {
        damageModifier = dm;
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
    /// IF OVERRIDE THIS, CALL BASE TO REMOVE STATUS EFFECT TO PAWN!
    /// </summary>
    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
    }

    /// <summary>
    /// Irrelevant for damage related status effects!
    /// They use the interface (by which they are differentiated)
    /// </summary>
    public override void Perform()
    {
        //Check duration?
    }

    public virtual float OperateOnDamage(float originalDamage)
    {
        //REDUCE DURATION here
        damageModifier.currentDuration--;
        return damageModifier.Operate(originalDamage);
    }
}