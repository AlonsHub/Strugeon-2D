using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumItemDisplayer : BasicDisplayer
{
     MagicItem magicItem;
    public void SetMeFull(MagicItem item)
    {
        magicItem = item;
        base.SetMe(new List<string> {magicItem.magicItemName, magicItem._EquipSlotType().ToString()}, new List<Sprite> { magicItem.itemSprite});
    }

    public void ClickMe()
    {
        SanctumSelectedPanel.Instance.SetMeFull(magicItem);
    }
}
