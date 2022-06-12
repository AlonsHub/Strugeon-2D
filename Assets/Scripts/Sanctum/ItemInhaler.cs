using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInhaler : MonoBehaviour
{
    ItemSpectrumProfile _playerSpectrumProfile;

    MagicItem _item;

    public void SelectItem(MagicItem selectedItem)
    {
        _item = selectedItem;
    }
    public void InhaleItem()
    {
        //start sequence:
        //chance to hit:

        //foreach (var item in collection)
        //{

        //}
        //float chanceToHit = 

        


    }

    bool RollChance(int x, int outOf)
    {
        return Random.Range(1,outOf+1) >= x;
    }
}
