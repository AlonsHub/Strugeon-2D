using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttacher : Attacher
{
    //[SerializeField]
    //int shieldAmount; //Only relevant if shields also have durations (if they wear off after some turns, regardless of their remainning shield amount
    //                  //needs to be implemented


    
    /// <summary>
    /// Performs the StatusEffectComponent.SetMe(Pawn target, string buffIconName, int newMaxTTL) and also sets the shield amount
    /// </summary>
    /// <param name="target"></param>
    /// <param name="buffIconName"></param>
    /// <param name="newMaxTTL"></param>
    /// <param name="fullShield"></param>
    //public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int fullShield)
    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int fullShield) //can/should have a SetMeFully
    {
        base.SetMe(target, buffIconName, newMaxTTL);
        ApplyEffect();
        attacherHP = fullShield;
    }
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        StartCoroutine(nameof(TTLPerTurn));
        tgtPawn.HasShield = true;
    }

    public override void RemoveEffect()
    {
        tgtPawn.HasShield = false;
        base.RemoveEffect();
    }
    
    IEnumerator TTLPerTurn()
    {
        while (ttl > 0)
        {
            yield return new WaitUntil(() => tgtPawn.TurnDone != true);

            ReduceTtlByOne();
            yield return new WaitUntil(() => tgtPawn.TurnDone == true);
        }
        RemoveEffect();
    }
}
