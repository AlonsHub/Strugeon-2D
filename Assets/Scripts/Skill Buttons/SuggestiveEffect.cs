using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Describes effects that have OnTurnStart/End behaviours, counters, cooldowns - any turn-based-based effect
/// </summary>
[System.Serializable]
public class SuggestiveEffect : StatusEffect
{
    public SuggestiveEffect(Pawn pawn, Sprite sprite) : base(pawn, sprite){}
    
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
