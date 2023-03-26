using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Describes effects that have OnTurnStart/End behaviours, counters, cooldowns - any turn-based-based effect
/// </summary>
[System.Serializable]
public class AfterActionWeightsEffect : StatusEffect
{
    public int totalDuration = 1;
    public int current;
    public AfterActionWeightsEffect(Pawn pawn, Sprite sprite) : base(pawn, sprite)
    {
        current = totalDuration;
    }
    public AfterActionWeightsEffect(Pawn pawn, Sprite sprite, int duration) : base(pawn, sprite)
    {
        totalDuration = duration;
        current = totalDuration;
    }
    
    /// <summary>
    /// MUST PERFORM THIS BASE!
    /// IF OVERRIDE THIS, CALL BASE TO ADD STATUS EFFECT TO PAWN!
    /// </summary>
    public override void ApplyEffect()
    {
        pawnToEffect.AddStatusEffect(this);
    }
    /// <summary>
    /// MUST PERFORM THIS BASE!
    /// </summary>
    public override void EndEffect()
    {
        //RemoveIconFromPawnBar();
        pawnToEffect.RemoveStatusEffect(this);
    }

    public override void Perform()
    {
        
    }

}
