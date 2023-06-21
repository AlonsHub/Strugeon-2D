using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FerocitySpell : SpellButton
{
    [SerializeField, Tooltip("Only uses the duration! damage will only be mitigated if takes target below 1 hp")]
    DamageModifier damageModifier;
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;


        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is FerocityEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is FerocityEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        new FerocityEffect(pawnTgt, effectIcon, damageModifier);

        base.OnButtonClick();
    }
}
