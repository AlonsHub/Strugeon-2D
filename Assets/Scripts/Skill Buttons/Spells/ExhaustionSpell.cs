﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionSpell : SpellButton
{
    [SerializeField, Tooltip("Only uses the duration! damage will only be mitigated if takes target below 1 hp")]
    DamageModifier damageModifier;
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;


        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is ExhaustionEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is ExhaustionEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        new ExhaustionEffect(pawnTgt, effectIcon, damageModifier);

        base.OnButtonClick();
    }
}
