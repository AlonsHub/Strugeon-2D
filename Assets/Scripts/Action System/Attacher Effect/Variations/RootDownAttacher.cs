using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDownAttacher : Attacher
{
    int minDamage, maxDamage, spikeDamage;
    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int fullHP, int minD, int maxD, int spikeDmg)
    {
        base.SetMe(target, buffIconName, newMaxTTL);
        ApplyEffect();
        minDamage = minD;
        maxDamage = maxD;
        spikeDamage = spikeDmg;
        attacherHP = fullHP;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        StartCoroutine(nameof(TTLPerTurn));

        if (tgtPawn.isEnemy)
        {
            //RefMaster.Instance.enemyInstances.Remove(tgtPawn);
            RefMaster.Instance.mercs.Add(tgtPawn);
        }
        else
        {
            RefMaster.Instance.enemyInstances.Add(tgtPawn);
            //RefMaster.Instance.mercs.Remove(tgtPawn);
        }
        tgtPawn.HasRoot = true;

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        if (tgtPawn.isEnemy)
        {
            //RefMaster.Instance.enemyInstances.Add(tgtPawn);
            RefMaster.Instance.mercs.Remove(tgtPawn);
        }
        else
        {
            RefMaster.Instance.enemyInstances.Remove(tgtPawn);
            //RefMaster.Instance.mercs.Add(tgtPawn);
        }
        tgtPawn.HasRoot = false;
    }

    IEnumerator TTLPerTurn()
    {
        while (ttl > 0)
        {
            yield return new WaitUntil(() => tgtPawn.TurnDone != true);

            if (attacherHP > 0)
                DamageHost(Random.Range(minDamage, maxDamage + 1)); //This will work, but needs to override the "shield" component of damage calculations

            ReduceTtlByOne();
            yield return new WaitUntil(() => tgtPawn.TurnDone == true);
        }

        if (attacherHP > 0)
            DamageHost(spikeDamage);

        RemoveEffect();
    }

    void DamageHost(int roll) //if ttl'ed out and not by HP reduction
    {
        tgtPawn.TakeDirectDamage(roll);

        BattleLogVerticalGroup.Instance.AddEntry("Root", ActionSymbol.Poison, tgtPawn.Name, roll, Color.green);
    }

    //public void 
}
