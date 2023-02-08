using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Describes effects that have OnTurnStart/End behaviours, counters, cooldowns - any turn-based-based effect
/// </summary>
[System.Serializable]
public abstract class SuggestiveEffect
{
    // What do?

    // Who do on?
    protected Pawn pawnToEffect;

    // What show?
    public Sprite iconSprite;

    
    public SuggestiveEffect(Pawn pawn)
    {
        pawnToEffect = pawn;
        iconSprite = null;
        //pawn.AddSuggestiveEffect(this); //may not want this for all suggestives
    }
    
    public SuggestiveEffect(Pawn pawn, Sprite sprite)
    {
        pawnToEffect = pawn;
        iconSprite = sprite;
        //pawn.AddSuggestiveEffect(this); //may not want this for all suggestives
    }

    /// <summary>
    /// Use this to: 
    /// 1) Set the actual effect.
    /// 2) Start/set counters, if any -> or engage a Destroy mechanism of any sort.
    /// ||| Warning! EFFECTS CAN NOT BE REMOVED! EFFECTS SHOULD ONLY REMOVE THEMSELVES!
    /// </summary>
    public abstract void ApplyEffect();
    /// <summary>
    /// After confirming the effect should end, this clears both the TurnInfo AND(?) the effect icon (?)
    /// </summary>
    public abstract void EndEffect();
    public abstract void Perform();
}
