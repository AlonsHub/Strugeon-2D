using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToughenSpirit : StatusEffectComponent
{
    [SerializeField]
    float damageModifier; //.5f
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        tgtPawn.DamageModifiers.Add(damageModifier);
    }

    //public override void SetMe(Pawn target, string buffIconName)
    //{
    //    base.SetMe(target, buffIconName);
    //    //target.OnTakeDamage += () => ttl--; //this is really cute, but honestly there should be a reduction method which check i

    //}
    /// <summary>
    /// Also performs base SetMe of StatusEffectComponent
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffIconName"></param>
    /// <param name="dmgMod"></param>
    public void SetFullEffect(Pawn target, string buffIconName, float dmgMod)
    {
        base.SetMe(target, buffIconName);
        tgtPawn.OnTakeDamage += () => ReduceTtlBy(1); //this is really cute, but honestly there should be a reduction method which check i
        damageModifier = dmgMod;
    }

    public override void ReduceTtlBy(int reduceBy)
    {
        base.ReduceTtlBy(reduceBy);
        if(ttl>0)
        {
            tgtPawn.DamageModifiers.Add(damageModifier);
        }
    }
}
