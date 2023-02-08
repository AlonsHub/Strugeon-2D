using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpathySpell : SkillButton
{
    Pawn targetPawn;


    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new EmpathyEffect(item, effectIcon, targetPawn);
        }
        //new CalmEffect(targetPawn, effectIcon);


        base.OnButtonClick();
    }
}
