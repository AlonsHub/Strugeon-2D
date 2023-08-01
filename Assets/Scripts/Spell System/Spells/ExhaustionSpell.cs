using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustionSpell : SpellButton
{
    [SerializeField]
    DamageModifier damageModifier;
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        //damageModifier.mod = modifier * .008f; //not sure TBD
        damageModifier.mod = modifier * .009f; //going for 10% at 100 modifier (nool capacity)

        Debug.LogError($"Exhaustion mods damage by {damageModifier.mod}");

        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is ExhaustionEffect).Any())
            {
                //StatusEffect se = pawnTgt.statusEffects.Where(s => s is ExhaustionEffect).SingleOrDefault();
                //se.StackMe(); 

                //This shouldn't really happen
                return;
            }
        }

        new ExhaustionEffect(pawnTgt, effectIcon, damageModifier);

        base.OnButtonClick();
    }

    public override void InteractableCheck()
    {
        base.InteractableCheck();
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is ExhaustionEffect).Any())
            {
                SetButtonInteractability(false);
            }
        }
    }
}
