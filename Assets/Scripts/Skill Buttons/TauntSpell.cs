using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntSpell : SkillButton
{
    Pawn targetPawn;

    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new TauntedEffect(item, effectIcon, targetPawn);
        }
        
        base.OnButtonClick();
    }
}