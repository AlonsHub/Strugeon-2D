using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable 
{
    MagicItem _Item(); //returns the item, for data purposes. All equpiables are Items
    IBenefit _Benefit();
    List<IBenefit> _Benefits(); //if relevant

    EquipSlotType _EquipSlotType();

    bool _IsEquipped();
    bool _InInventory(); //not sure I need this, 
                        //but in case there's an item shop or a way to view items 
                        //that are niether equipped nor in-inventory

}
