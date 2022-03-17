using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootDownAttacher : Attacher
{
    public void SetMeFull(Pawn target, string buffIconName, int newMaxTTL, int fullHP)
    {
        base.SetMe(target, buffIconName, newMaxTTL);
        ApplyEffect();
        attacherHP = fullHP;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        StartCoroutine(nameof(TTLPerTurn));

        if (tgtPawn.isEnemy)
        {
            RefMaster.Instance.enemyInstances.Remove(tgtPawn);
            RefMaster.Instance.mercs.Add(tgtPawn);
        }
        else
        {
            RefMaster.Instance.enemyInstances.Add(tgtPawn);
            RefMaster.Instance.mercs.Remove(tgtPawn);
        }
        tgtPawn.HasRoot = true;

    }

    public override void RemoveEffect()
    {
        base.RemoveEffect();
        if (tgtPawn.isEnemy)
        {
            RefMaster.Instance.enemyInstances.Add(tgtPawn);
            RefMaster.Instance.mercs.Remove(tgtPawn);
        }
        else
        {
            RefMaster.Instance.enemyInstances.Remove(tgtPawn);
            RefMaster.Instance.mercs.Add(tgtPawn);
        }
        tgtPawn.HasRoot = false;
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

    //public void 
}
