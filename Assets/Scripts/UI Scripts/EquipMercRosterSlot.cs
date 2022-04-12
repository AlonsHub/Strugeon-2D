using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMercRosterSlot : BasicDisplayer
{
    public int index;
    public MercPoolDisplayer mercPoolDisplayer;
   

    public void MyOnClick()
    {
        mercPoolDisplayer.OpenGearDisplayerByMercIndex(index);
    }
}
