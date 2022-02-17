using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSet
{
    public List<MagicItemSO> setItemSOs = new List<MagicItemSO>(); //Refs to SOs of all items in set
    public List<int> perItemWeight = new List<int>();

    public ItemSet()
    {

    }
    public ItemSet(List<MagicItemSO> itemSOs, List<int> weights)
    {
        setItemSOs = itemSOs;
        perItemWeight = weights;
    }
}
