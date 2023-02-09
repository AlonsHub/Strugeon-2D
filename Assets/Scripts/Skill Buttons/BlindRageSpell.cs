using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindRageSpell : SkillButton
{
    Pawn targetPawn;


    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            //new (item, effectIcon);
        }
        //new CalmEffect(targetPawn, effectIcon);


        base.OnButtonClick();
    }
}
