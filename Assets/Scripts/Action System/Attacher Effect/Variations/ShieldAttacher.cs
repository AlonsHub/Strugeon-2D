using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttacher : Attacher
{
    //[SerializeField]
    //int shieldAmount; //Only relevant if shields also have durations (if they wear off after some turns, regardless of their remainning shield amount
                      

    /// <summary>
    /// Performs the StatusEffectComponent.SetMe(Pawn target, string buffIconName, int newMaxTTL) and also sets the shield amount
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffIconName"></param>
    /// <param name="newMaxTTL"></param>
    /// <param name="fullShield"></param>
    //public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int fullShield)
    public override void SetMe(Pawn target, string buffIconName, int newMaxTTL)
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
    public override int ReduceTtlBy(int reduceBy)
    {
        return base.ReduceTtlBy(reduceBy);
    }

    //private void OnDisable()
    //{
    //    if (ttl < 0) //when this is destoryed (ReduceTtlBy() will destroy it if it comes under 0)
    //    {
    //        //ttl is shieldAmount - so if ttl is under 0 - damage needs to carry over
    //    }
    //}
}
