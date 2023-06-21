using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CalmSpell : SpellButton
{
    [SerializeField]
    int duration = 2;
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is CalmEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is CalmEffect).SingleOrDefault();
                se.StackMe();
                return;
            }
        }
        new CalmEffect(pawnTgt, effectIcon, duration);
       
        base.OnButtonClick();
    }
}