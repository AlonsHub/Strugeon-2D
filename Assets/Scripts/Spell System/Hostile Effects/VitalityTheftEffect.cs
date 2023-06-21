using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VitalityTheftEffect : MonsterStatusEffect, I_StatusEffect_TurnStart
{
    int totalDuration = 2;
    int current;

    int minDamage, maxDamage;

    List<Pawn> _pawnsHealedThisRound;

    [SerializeField]
    SpecialCollider specialCollider;
    //Range or collider?
    //public int RolledDamage => Random.Range(minDamage, maxDamage);
    public int RolledDamage => Random.Range(minDamage, maxDamage);

    public VitalityTheftEffect(Pawn target, Sprite sprite, Pawn src, SpecialCollider sc) : base(target, sprite, src)
    {
        alignment = EffectAlignment.Negative;
        
        specialCollider = sc;

        if (target.statusEffects != null && target.statusEffects.Count > 0)
        {
            StatusEffect existingEffect = target.statusEffects.Where(x => x is VitalityTheftEffect).FirstOrDefault();
            if (existingEffect != null)
            {
                (existingEffect as VitalityTheftEffect).StackMe(); //THIS SHOULD BE STACKME()!
                return; //Delete self?
            }
        }

        _pawnsHealedThisRound = new List<Pawn>();
        current = totalDuration;

        //Instantiate collider thing (with OnTriggerEnter-> which heals and adds the healed pawn to _pawnsHealedThisRound

        BattleLogVerticalGroup.Instance.AddEntry(src.Name, ActionSymbol.Death, target.Name); //make vitality theft symbol

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
        //int dmg = Random.Range(minDamage, maxDamage);
        //pawnToEffect.TakeElementalDamage(dmg, SturgeonColours.Instance.posionGreen);
        //BattleLogVerticalGroup.Instance.AddEntry(source.Name, ActionSymbol.Poison, pawnToEffect.Name, dmg, SturgeonColours.Instance.posionGreen);

        List<Pawn> _targets = new List<Pawn>(); //GET OVERLAP TARGETS

        int dmg = RolledDamage;

        //cycle through tarhets
        foreach (var item in _targets)
        {
            if (_pawnsHealedThisRound.Contains(item))
                continue;

            item.Heal(dmg);
        }

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
