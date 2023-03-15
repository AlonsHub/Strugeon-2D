using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ReflectSpell : SpellButton
{
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        if(pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is ReflectEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is ReflectEffect).SingleOrDefault();
                se.StackMe();
                return;
            }
        }
        new ReflectEffect(pawnTgt, effectIcon);

        base.OnButtonClick();
    }
}
