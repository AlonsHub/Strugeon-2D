using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CharmEffect : EndOfTurnStatusEffect
{
    WeaponItem wi;

    int totalDuration = 2;
    int current;
    public CharmEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        alignment = EffectAlignment.Negative;
        if (target.statusEffects != null && target.statusEffects.Count > 0)
        {
            StatusEffect existingEffect = target.statusEffects.Where(x => x is CharmEffect).FirstOrDefault();
            if (existingEffect != null)
            {
                (existingEffect as CharmEffect).RefreshCounter();
                return; //Delete self?
            }
        }
        current = 0;
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

        base.ApplyEffect();
    }

    public override void EndEffect()
    {
        if (pawnToEffect.isEnemy)
            wi.targets = RefMaster.Instance.mercs;
        else
            wi.targets = RefMaster.Instance.enemyInstances;
        base.EndEffect();
    }

    public override void Perform()
    {
        current++;
        if (current >= totalDuration)
        {
            EndEffect();
        }
    }

    public void RefreshCounter()
    {
        current = totalDuration;
    }
}
