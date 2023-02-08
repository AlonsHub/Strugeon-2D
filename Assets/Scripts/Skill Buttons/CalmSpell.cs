using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmSpell : SkillButton
{
    Pawn targetPawn;

   
    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        new CalmEffect(targetPawn, effectIcon);
       

        base.OnButtonClick();
    }
}
