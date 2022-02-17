using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSet
{
    public List<MagicItemSO> itemSOs; //Refs to SOs of all items in set
    public List<int> perItemWeight;
}
