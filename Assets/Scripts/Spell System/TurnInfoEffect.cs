using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Describes effects that have OnTurnStart/End behaviours, counters, cooldowns - any turn-based-based effect
/// </summary>
[System.Serializable]
public abstract class TurnInfoEffect  : StatusEffect
{
    // What do?

    // Who do on?
    protected TurnInfo turnInfoToEffect; 
    
    // Counters?

    // End *Condition* - TBD condition class -> a general class that can, as with predicates, assert general statements.

    public TurnInfoEffect(TurnInfo ti, Sprite s) : base(ti.TryGetPawn(), s)
    {
        turnInfoToEffect = ti;
    }
    public TurnInfoEffect(TurnInfo ti) : base(ti.TryGetPawn())
    {
        turnInfoToEffect = ti;
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
}
