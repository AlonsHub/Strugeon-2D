using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraverySpell : SkillButton
{
    //Pawn pawnTgt;
    

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        new BraveryEffect(pawnTgt, effectIcon);
    

        base.OnButtonClick();
    }
}
