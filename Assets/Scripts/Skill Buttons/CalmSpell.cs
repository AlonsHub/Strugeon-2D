using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmSpell : SkillButton
{
    //Pawn targetPawn;

   
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        new CalmEffect(pawnTgt, effectIcon);
       

        base.OnButtonClick();
    }
}
