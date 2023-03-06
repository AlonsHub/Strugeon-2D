using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BraveryEffect : SuggestiveEffect
{
    /// <summary>
    /// Adds itself to a list of SuggestiveEffects on the pawn - to be analyzed just after actions were calculated!
    /// </summary>
    /// <param name="pawn"></param>
    public BraveryEffect(Pawn pawn, Sprite s) : base(pawn, s)
    {
        alignment = EffectAlignment.Positive;
        ApplyEffect();
    }

    //Just after Action Weights calc
    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);
        //AddIconToPawnBar();

        //add icon symbol and spawn gfx/vfx w/efx
    }

    //After action was rolled-on and decided
    public override void EndEffect()
    {
        //RemoveIconFromPawnBar();
        pawnToEffect.RemoveStatusEffect(this);

        //remove icon symbol and whatnot
    }
    //IEnumerator RemoveEffectFromPawn()
    //{
    //    yield return new WaitForEndOfFrame();
    //    yield return new WaitForEndOfFrame();
    //}

    public override void Perform()
    {
        pawnToEffect.actionPool.Remove(pawnToEffect.actionPool.Where(x => x.relevantItem is EscapeItem).FirstOrDefault());

        EndEffect();
    }

   
}
