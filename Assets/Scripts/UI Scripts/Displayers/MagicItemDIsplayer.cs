using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicItemDIsplayer : BasicDisplayer
{
    public MagicItem magicItem;

    public void SetItem(MagicItem newItem)
    {
        magicItem = newItem;

        SetMe(magicItem.magicItemName, magicItem.itemSprite);
    }
}
