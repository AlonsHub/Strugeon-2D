using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RootDownAttacher : Attacher
{
    public int minDamage, maxDamage, spikeDamage;
    public RootDownEffect rde;

    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, GameObject rootVisual, int fullHP, int minD, int maxD, int spikeDmg)
    {
        base.SetMeWithVFX(target, buffIconName, newMaxTTL, rootVisual);
        ApplyEffect();
        minDamage = minD;
        maxDamage = maxD;
        spikeDamage = spikeDmg;
        attacherHP = fullHP;

    }
    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, GameObject rootVisual, int fullHP, int minD, int maxD, int spikeDmg, RootDownEffect rootDownEffect)
    {
        base.SetMeWithVFX(target, buffIconName, newMaxTTL, rootVisual);
        ApplyEffect();

        rde = rootDownEffect;

        minDamage = minD;
        maxDamage = maxD;
        spikeDamage = spikeDmg;
        attacherHP = fullHP;

    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        //StartCoroutine(nameof(TTLPerTurn));

        if (tgtPawn.isEnemy)
        {
            RefMaster.Instance.mercs.Add(tgtPawn);
        }
        else
        {
            RefMaster.Instance.enemyInstances.Add(tgtPawn);
        }
        tgtPawn.HasRoot = true;
    }

    public override void RemoveEffect()
    {
        //if (tgtPawn.isEnemy)
        //{
        //    RefMaster.Instance.mercs.Remove(tgtPawn);
        //}
        //else
        //{
        //    RefMaster.Instance.enemyInstances.Remove(tgtPawn);
        //}
        //tgtPawn.HasRoot = false;
        rde.EndEffectFromAttacher();
        base.RemoveEffect();
    }


    public void DamageHost(int roll) //if ttl'ed out and not by HP reduction
    {
        tgtPawn.TakeDirectDamage(roll);

        BattleLogVerticalGroup.Instance.AddEntry("Root", ActionSymbol.Poison, tgtPawn.Name, roll, Color.green);
    }

    //public void 
}
