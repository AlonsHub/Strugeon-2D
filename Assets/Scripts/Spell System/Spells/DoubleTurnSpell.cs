using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoubleTurnSpell : SpellButton
{
    //private Pawn targetPawn;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;



        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is DoubleTurn_Effect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is DoubleTurn_Effect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }



        DoubleTurn_Effect doubleTurn_Effect = new DoubleTurn_Effect(pawnTgt, effectIcon);
      

        base.OnButtonClick();
    }
}
