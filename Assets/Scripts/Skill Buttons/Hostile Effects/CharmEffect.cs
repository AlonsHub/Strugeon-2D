using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmEffect : EndOfTurnStatusEffect
{
    WeaponItem wi;

    int totalDuration = 2;
    int current;
    public CharmEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
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
}
