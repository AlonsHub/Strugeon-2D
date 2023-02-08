using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraverySpell : SkillButton
{
    Pawn targetPawn;
    

    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        new BraveryEffect(targetPawn, effectIcon);
    

        base.OnButtonClick();
    }
}
