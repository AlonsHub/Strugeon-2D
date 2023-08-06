using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuggestSpell : SpellButton
{
    //private Pawn targetPawn;
    public float slowerTime;
    public PurpleChoosingMode purpleChoosingMode;

    public MouseBehaviour mouseBehaviour;



    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        purpleChoosingMode.gameObject.SetActive(true);
        //Time.timeScale = slowerTime;
        TimeChanger.Instance.ToggleTimePause(true);
        purpleChoosingMode.Setup(pawnTgt);
        mouseBehaviour.HideMenus();

        base.OnButtonClick();
    }

    public override void InteractableCheck()
    {
        base.InteractableCheck();
        if (MouseBehaviour.hitTarget.statusEffects != null && MouseBehaviour.hitTarget.statusEffects.Count != 0)
        {
            if (MouseBehaviour.hitTarget.statusEffects.Where(s => s is SuggestEffect).Any())
            {
                SetButtonInteractability(false);
            }
        }
    }
}
