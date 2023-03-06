using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectAlignment {Positive, Negative, Nuetral}; //nuetral could be "indifferent"
public abstract class StatusEffect
{

    protected Pawn pawnToEffect;

    public Sprite iconSprite;
    public EffectAlignment alignment;

    //Temp - change this when "casting spells" replaces the SkillButton method. So ALL effect castings
    //will be "smarter" events with knowledge of source as well as target
    /// <summary>
    /// Null if psion
    /// </summary>
    public Pawn caster; 


    public StatusEffect(Pawn target)
    {
        pawnToEffect = target;
        iconSprite = null;
    }
    public StatusEffect(Pawn target, Sprite sprite)
    {
        pawnToEffect = target;
        iconSprite = sprite;
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

    /// <summary>
    /// The logic to be called when a turntaker already has a statusEffect like this on them.
    /// Destroy previous and make new one or topup the previous and forget about the new one
    /// </summary>
    public virtual void StackMe()
    {

    }
}