using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BraverySpell : SpellButton
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

    public override void InteractableCheck()
    {
        base.InteractableCheck();
        
        if (MouseBehaviour.hitTarget.statusEffects != null && MouseBehaviour.hitTarget.statusEffects.Count != 0)
        {
            if (MouseBehaviour.hitTarget.statusEffects.Where(s => s is BraveryEffect).Any())
            {
                SetButtonInteractability(false);
            }
        }
    }
}
