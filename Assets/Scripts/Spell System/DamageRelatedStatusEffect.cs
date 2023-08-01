using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageRelatedStatusEffect : StatusEffect
{
    [SerializeField]
    protected DamageModifier damageModifier;

    public DamageRelatedStatusEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite)
    {
        damageModifier = dm;
        damageModifier.currentDuration = damageModifier.totalDuration;
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
    /// override this to chnage the way duration is reduced
    /// </summary>
    public override void Perform()
    {
        if (damageModifier.currentDuration != -100) //temp TBF this is how we do infinite duration damage modifiers atm
            return;
        damageModifier.currentDuration--;
        if (damageModifier.currentDuration <= 0)
        {
            EndEffect();
        }
    }

    public virtual float OperateOnDamage(float originalDamage)
    {
        //REDUCE DURATION here
        //damageModifier.currentDuration--;
        return damageModifier.Operate(originalDamage);
    }
}