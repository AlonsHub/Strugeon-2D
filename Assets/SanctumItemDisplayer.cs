using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumItemDisplayer : BasicDisplayer
{
    [SerializeField]
    SacntumItemHover sacntumItemHover;
    public MagicItem magicItem;
    public void SetMeFull(MagicItem item)
    {
        magicItem = item;
        base.SetMe(new List<string> {magicItem.magicItemName, magicItem._EquipSlotType().ToString()}, new List<Sprite> { magicItem.itemSprite});
        sacntumItemHover.SetToItem(item);
    }

    public void ClickMe()
    {
        SanctumSelectedPanel.Instance.SetMeFull(magicItem);
    }
}
