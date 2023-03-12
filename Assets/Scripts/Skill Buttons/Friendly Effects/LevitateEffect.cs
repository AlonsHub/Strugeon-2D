using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateEffect : StatusEffect, I_StatusEffect_TurnEnd
{
    int currentDuration;
    int totalDuration;

    //This effect ADDS to the distance to and from the pawn it affects to prevent melee attacks by and against the target
    public LevitateEffect(Pawn target, Sprite sprite, int duration) : base(target, sprite)
    {
        alignment = EffectAlignment.Positive;

        totalDuration = duration;
        currentDuration = totalDuration;

        ApplyEffect();
    }
    public override void ApplyEffect()
    {
        pawnToEffect.tileWalker.SetElevation(28); //14 * 2

        pawnToEffect.AddStatusEffect(this);
        //Similar to srhink GFX

        pawnToEffect.SetGFXScale(new Vector3(1.2f, 1.2f, 1f));
    }
    public override void EndEffect()
    {
        pawnToEffect.tileWalker.SetElevation(0);

        pawnToEffect.SetGFXScale(new Vector3(1f, 1f, 1f));

        pawnToEffect.RemoveStatusEffect(this);

        //Undo gfx
    }
    public override void Perform()
    {
        //duration and what else?
        currentDuration--;
        if (currentDuration <= 0)
        {
            EndEffect();
        }
    }
}
