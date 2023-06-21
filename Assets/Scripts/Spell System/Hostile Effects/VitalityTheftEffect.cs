using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VitalityTheftEffect : StatusEffect, I_StatusEffect_TurnStart
{
    int totalDuration = 3;
    int current;

    //int minDamage, maxDamage;
    float damage;
    List<Pawn> _pawnsHealedThisRound => specialCollider.GetHealedThisRound;

    SpecialCollider specialCollider;

    
    public VitalityTheftEffect(Pawn target, Sprite sprite, float dmg, SpecialCollider sc) : base(target, sprite)
    {
        alignment = EffectAlignment.Negative;
        damage = dmg;
        specialCollider = sc;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        if (pawnToEffect.statusEffects != null && pawnToEffect.statusEffects.Count > 0)
        {
            StatusEffect existingEffect = pawnToEffect.statusEffects.Where(x => x is VitalityTheftEffect).FirstOrDefault();
            if (existingEffect != null)
            {
                (existingEffect as VitalityTheftEffect).StackMe(); //THIS SHOULD BE STACKME()!
                return; //Delete self?
            }
        }

        //ADD Collider Component and Init() it with damage
        current = totalDuration;

        pawnToEffect.AddStatusEffect(this);


    }

    public override void EndEffect()
    {
        pawnToEffect.RemoveStatusEffect(this);
        specialCollider.SelfDestruct();
    }

    public override void Perform()
    {
        //int dmg = Random.Range(minDamage, maxDamage);
        //pawnToEffect.TakeElementalDamage(dmg, SturgeonColours.Instance.posionGreen);
        //BattleLogVerticalGroup.Instance.AddEntry(source.Name, ActionSymbol.Poison, pawnToEffect.Name, dmg, SturgeonColours.Instance.posionGreen);

        List<Pawn> _targets = new List<Pawn>(); //GET OVERLAP TARGETS

        //int dmg = damage;

        //cycle through tarhets
        foreach (var item in _targets)
        {
            if (_pawnsHealedThisRound.Contains(item))
                continue;

            item.Heal((int)damage);
        }

        _pawnsHealedThisRound.Clear();

        current--;
        if (current <= 0)
        {
            EndEffect();
        }
    }

    public override void StackMe()
    {
        current = totalDuration;
    }
}
