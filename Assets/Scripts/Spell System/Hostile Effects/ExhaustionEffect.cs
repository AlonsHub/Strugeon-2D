using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionEffect : DamageRelatedStatusEffect, I_StatusEffect_TurnEnd, I_StatusEffect_OutgoingDamageMod
{
    WeaponItem wi;
    public ExhaustionEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, sprite, dm)
    {
        alignment = EffectAlignment.Negative;
        //damageModifier.currentDuration = -100; //just making sure this is permanent. //Instead, not calling base.Perform() is the better way
        wi = pawnToEffect.GetWeaponItem;


        ApplyEffect();
    }

    public override void Perform()
    {
        //Debug.LogWarning($"{pawnToEffect.Name} exahusted! {damageModifier.currentDuration}");

        wi.maxDamage = (int)(wi.maxDamage * damageModifier.mod);
        wi.minDamage = (int)(wi.minDamage * damageModifier.mod);
        Debug.LogWarning($"{pawnToEffect.Name} exahusted! Min damage is now {wi.minDamage} and Man damage is now {wi.maxDamage}");
        //damageModifier.mod *= .009f; //reduces damage mod for NEXT time
        //base.Perform(); //Not calling base.Perform() IS THE BEST WAY TO HAVE AN INFINITE DURATION!
    }

}
