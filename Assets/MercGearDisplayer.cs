using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercGearDisplayer : BasicDisplayer
{
    
    GearSlotDispayer[] gearSlots = new GearSlotDispayer[3]; // by EquipSlotType
    MercSheet mercSheet;

    public void SetMeFully(MercSheet ms)
    {
        mercSheet = ms;
    }
}
