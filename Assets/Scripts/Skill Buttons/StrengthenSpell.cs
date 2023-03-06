using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StrengthenSpell : SkillButton
{
    public DamageModifier damageModifier;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
       
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is StrengthenEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is StrengthenEffect).SingleOrDefault();
                se.StackMe();
                return;
            }
        }
        new StrengthenEffect(pawnTgt, effectIcon,damageModifier); //damageModifier comes from DamageModifyingSpell

        base.OnButtonClick();
    }
}