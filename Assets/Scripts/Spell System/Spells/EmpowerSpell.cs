using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EmpowerSpell : SpellButton
{
    public DamageModifier damageModifier;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
       
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is EmpowerEffect).Any())
            {
                //No need to stack a skill that lasts till end of round

                //StatusEffect se = pawnTgt.statusEffects.Where(s => s is EmpowerEffect).SingleOrDefault();
                //se.StackMe();
                return;
            }
        }

        damageModifier.mod = modifier * .0014f;
        new EmpowerEffect(pawnTgt, effectIcon,damageModifier); //damageModifier comes from DamageModifyingSpell

        base.OnButtonClick();
    }

    public override void InteractableCheck()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        base.InteractableCheck();
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is EmpowerEffect).Any())
            {
                SetButtonInteractability(false);
            }
        }
    }
}