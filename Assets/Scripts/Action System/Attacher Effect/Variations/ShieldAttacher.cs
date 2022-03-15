using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttacher : Attacher
{
    //[SerializeField]
    //int shieldAmount; //Only relevant if shields also have durations (if they wear off after some turns, regardless of their remainning shield amount
                      //needs to be implemented

    /// <summary>
    /// Performs the StatusEffectComponent.SetMe(Pawn target, string buffIconName, int newMaxTTL) and also sets the shield amount
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffIconName"></param>
    /// <param name="newMaxTTL"></param>
    /// <param name="fullShield"></param>
    //public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int fullShield)
    public override void SetMe(Pawn target, string buffIconName, int newMaxTTL) //can/should have a SetMeFully
    {
        base.SetMe(target, buffIconName, newMaxTTL);
        ApplyEffect();
        //shieldAmount = fullShield;
    }
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        tgtPawn.HasShield = true;
    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        tgtPawn.HasShield = false;
    }
    /// <summary>
    /// Returns int of the remainning ttl
    /// </summary>
    /// <param name="reduceBy"></param>
    /// <returns></returns>
    public override int ReduceTtlBy(int reduceBy) //is reduced as HP when hit (in Pawn TakeDamage() logic)
    {
        return base.ReduceTtlBy(reduceBy);
    }
}
