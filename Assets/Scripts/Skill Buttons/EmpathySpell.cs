using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpathySpell : SkillButton
{


    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new EmpathyEffect(item, effectIcon, pawnTgt);
        }

        base.OnButtonClick();
    }
}
