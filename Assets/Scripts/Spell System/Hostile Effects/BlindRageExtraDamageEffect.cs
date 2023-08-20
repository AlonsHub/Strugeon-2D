using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindRageExtraDamageEffect : DamageRelatedStatusEffect, I_StatusEffect_OutgoingDamageMod
{
    public BlindRageExtraDamageEffect(Pawn target, Sprite sprite, DamageModifier dm) : base(target, null, dm) //pass null instead of sprite just to ensure it won't add a status icon by accident
    {
        //TBD TBF about "Buff Bundles" and whether they should be treated as a single buff?
        alignment = EffectAlignment.Negative; //DEBATEABLE! since it doesn't have its own icon, it would be weird to cleanse a target of blind rage and for it to keep the damage buff... since they are one "buff bundle" that should deactivate as a whole.


        ApplyEffect();
    }

    

}
