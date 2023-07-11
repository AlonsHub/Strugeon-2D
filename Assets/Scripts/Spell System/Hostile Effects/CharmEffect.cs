using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CharmEffect : MonsterStatusEffect, I_StatusEffect_TurnEnd
{
    WeaponItem wi;

    int totalDuration = 2;
    int current;
    public CharmEffect(Pawn target, Sprite sprite, Pawn src) : base(target, sprite, src)
    {
        alignment = EffectAlignment.Negative;

        //This happens here since the spell is cast by Mavka and not a SkillButton/PsionSpell as usual.
        // Spell casting should be a thing in-and-of-itself so that their targeting could be reflected for example...
        //but also to centeralized these checks

        if (target.statusEffects != null && target.statusEffects.Count > 0)
        {
            StatusEffect existingEffect = target.statusEffects.Where(x => x is CharmEffect).FirstOrDefault();
            if (existingEffect != null)
            {
                (existingEffect as CharmEffect).RefreshCounter(); //THIS SHOULD BE STACKME()!
                return; //Delete self?
            }
        }
        current = totalDuration;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
          wi = pawnToEffect.GetComponent<WeaponItem>();
        if(!wi)
        {
            Debug.LogError("Weapon item not found");
            return;
        }
        if (pawnToEffect.isEnemy)
            wi.targets = RefMaster.Instance.enemyInstances;
        else
            wi.targets = RefMaster.Instance.mercs;

        pawnToEffect.AddStatusEffect(this);
    }

    public override void EndEffect()
    {
        //THIS SHOULD RE-SET the reference back to the Pawns.targets expression TBF TBD
        if (pawnToEffect.isEnemy)
            wi.targets = RefMaster.Instance.mercs;
        else
            wi.targets = RefMaster.Instance.enemyInstances;

        pawnToEffect.RemoveStatusEffect(this);
    }

    public override void Perform()
    {
        current--;
        if (current <= 0)
        {
            EndEffect();
        }
    }

    public void RefreshCounter() //Stack me? TBD TBF
    {
        current = totalDuration;
    }
}
