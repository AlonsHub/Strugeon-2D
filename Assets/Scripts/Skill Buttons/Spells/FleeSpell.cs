using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeSpell : SpellButton
{
    
    //DONE!
    public override void OnButtonClick()
    {
        TurnMachine.Instance.Run();
     
        base.OnButtonClick();
    }

}