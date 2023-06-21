using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindEffect : EndOfTurnStatusEffect
{
    WeaponItem wi;

    int totalDuration = 2;
    int current;

    Vector2Int damage;
    public BlindEffect(Pawn target, Sprite sprite) : base(target, sprite)
    {
        alignment = EffectAlignment.Negative;
        current = 0;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        wi = pawnToEffect.GetComponent<WeaponItem>();
        if (!wi)
        {
            Debug.LogError("Weapon item not found");
            return;
        }
        damage.x = wi.maxDamage;
        damage.y = wi.minDamage;
        damage.x = 0;
        damage.y = 0;
        base.ApplyEffect();
    }

    public override void EndEffect()
    {
        wi.maxDamage = damage.x;
        wi.minDamage = damage.y;

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
