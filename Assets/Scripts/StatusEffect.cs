using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectAlignment {Positive, Negative, Nuetral}; //nuetral could be "indifferent"
public abstract class StatusEffect
{

    protected Pawn pawnToEffect;

    public Sprite iconSprite;
    public EffectAlignment alignment;


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

    //public void AddIconToPawnBar()
    //{
    //    pawnToEffect.AddEffectIcon(iconSprite, this.GetType().ToString());
    //}
    //public void RemoveIconFromPawnBar()
    //{
    //    pawnToEffect.RemoveIconByName(this.GetType().ToString());
    //}
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