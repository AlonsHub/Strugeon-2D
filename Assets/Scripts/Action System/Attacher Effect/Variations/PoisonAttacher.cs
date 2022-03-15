using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttacher : Attacher
{
    int minDamage, maxDamage;

    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int minDmg, int maxDmg)
    {
        minDamage = minDmg;
        maxDamage = maxDmg;
        base.SetMe(target, buffIconName, newMaxTTL);
        ApplyEffect(); 
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        StartCoroutine(nameof(DamagePerTurn));
    }
    
    IEnumerator DamagePerTurn()
    {
        while (ttl > 0)
        {
            yield return new WaitUntil(() => tgtPawn.TurnDone != true);
            
            tgtPawn.TakeElementalDamage(Random.Range(minDamage, maxDamage), Color.green);//
            ReduceTtlByOne();
        }
        RemoveEffect();
    }

}
