using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindRageSpell : SkillButton
{
    //Pawn targetPawn;


    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        
        new BlindRageEffect(pawnTgt, effectIcon);


        base.OnButtonClick();
    }
}
