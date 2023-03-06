using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlindRageEffect : SuggestiveEffect
{
    public BlindRageEffect(Pawn pawn, Sprite sprite) : base(pawn, sprite)
    {
        alignment = EffectAlignment.Negative;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);
    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
    }

    public override void Perform()
    {
        List<Pawn> allPawns = new List<Pawn> (RefMaster.Instance.enemyInstances);
        List<Pawn> adjPawns = new List<Pawn>();
        allPawns.AddRange(RefMaster.Instance.mercs);
        allPawns.Remove(pawnToEffect); // can't hit himself

        foreach (var item in allPawns)
        {
            if(pawnToEffect.tileWalker.currentNode.GetDistanceToTarget(item.tileWalker.currentNode) <= 14)
            {
                adjPawns.Add(item);
            }
        }
        if(adjPawns.Count == 0)
        {
            //Debug.LogError("no adj pawns");
            return; //keeping the effect ON
        }

        pawnToEffect.actionPool.Clear();
        Pawn target = adjPawns[Random.Range(0, adjPawns.Count)];

        pawnToEffect.actionPool.Add(new ActionVariation(pawnToEffect.GetComponent<WeaponItem>(), target.gameObject, 1000));

        //also! needs to add damage to this strike!

        EndEffect();
    }

    

}
