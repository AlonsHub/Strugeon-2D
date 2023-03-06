using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VenomEffect : MonsterStatusEffect, I_StatusEffect_TurnStart
{
    int totalDuration = 2;
    int current;

    int minDamage, maxDamage;

    public VenomEffect(Pawn target, Sprite sprite, Pawn src, int minD, int maxD) : base(target, sprite, src)
    {
        alignment = EffectAlignment.Negative;
        minDamage = minD;
        maxDamage = maxD;

        if (target.statusEffects != null && target.statusEffects.Count > 0)
        {
            StatusEffect existingEffect = target.statusEffects.Where(x => x is VenomEffect).FirstOrDefault();
            if (existingEffect != null)
            {
                (existingEffect as VenomEffect).StackMe(); //THIS SHOULD BE STACKME()!
                return; //Delete self?
            }
        }
        BattleLogVerticalGroup.Instance.AddEntry(src.Name, ActionSymbol.Poison, target.Name);

        current = totalDuration;
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
        int dmg = Random.Range(minDamage, maxDamage);
        pawnToEffect.TakeElementalDamage(dmg, SturgeonColours.Instance.posionGreen);
        BattleLogVerticalGroup.Instance.AddEntry(source.Name, ActionSymbol.Poison, pawnToEffect.Name, dmg, SturgeonColours.Instance.posionGreen);

        current--;
        if (current <= 0)
        {
            EndEffect();
        }
    }

    public override void StackMe()
    {
        current = totalDuration;
        //base.StackMe();
    }
}
