using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TauntSpell : SpellButton
{
    [SerializeField]
    int duration = 1;
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new TauntedEffect(item, effectIcon, pawnTgt, duration);
        }
        
        base.OnButtonClick();
    }
}