using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BraverySpell : SkillButton
{
    //Pawn pawnTgt;
    

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is BraveryEffect).Any())
            {
                Debug.LogError($"Spell failed! This target already has {this.GetType()} in effect.");
                return;
            }
        }
        new BraveryEffect(pawnTgt, effectIcon);
    

        base.OnButtonClick();
    }
}
