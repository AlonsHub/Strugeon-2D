using System.Collections;
using System.Collections.Generic;
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
}
