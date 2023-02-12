using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindRageSpell : SkillButton
{
    Pawn targetPawn;


    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        
        new BlindRageEffect(targetPawn, effectIcon);


        base.OnButtonClick();
    }
}
