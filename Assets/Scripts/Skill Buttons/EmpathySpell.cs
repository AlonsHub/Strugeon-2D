using System.Collections;
using System.Linq;

using System.Collections.Generic;
using UnityEngine;

public class EmpathySpell : SkillButton
{


    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;


        //TBF - this is kind of waiting on a game design decision
        //if (pawnTgt.statusEffects.Where(s => s is EmpathyEffect).Any())
        //{
        //    Debug.LogError($"Spell failed! This target already has {this.GetType()} in effect.");
        //    return;
        //}

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new EmpathyEffect(item, effectIcon, pawnTgt);
        }

        base.OnButtonClick();
    }
}
