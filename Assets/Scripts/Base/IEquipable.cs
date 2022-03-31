using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable 
{
    MagicItem _Item(); //returns the item, for data purposes. All equpiables are Items
    IBenefit _Benefit();
    List<IBenefit> _Benefits(); //if relevant

    EquipSlotType _EquipSlotType();

}
