using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only unique for being reflectable at the moment, but also the only spells that really require a "source"
/// </summary>
public class MonsterStatusEffect : StatusEffect //No interfaces needed - it cannot be performed
{
    public Pawn source;
    public MonsterStatusEffect(Pawn target, Sprite sprite, Pawn src) : base(target, sprite)
    {
        source = src;

        if(target.GetSingleStatusEffectByPredicate(x => x is ReflectEffect) != null) // make an "Any()" variation that returns bool
        {
            //THIS PREFORMS THE REFLECTIONS! Reflect basically changes the target to be the source, at the "casting" level.
            target = src;
            pawnToEffect = src;
        }
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
        throw new System.NotImplementedException();
    }
}
