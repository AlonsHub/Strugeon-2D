using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumItemDisplayer : BasicDisplayer
{
    //[SerializeField]
    //SacntumItemHover sacntumItemHover;
    public MagicItem magicItem;

    [SerializeField]
    ClassEggPanel classEggPanel;

    public void SetMeFull(MagicItem item)
    {
        magicItem = item;
        base.SetMe(new List<string> {magicItem.magicItemName, magicItem._EquipSlotType().ToString()}, new List<Sprite> { magicItem.itemSprite});
        classEggPanel.SetEggs(item.relevantClasses);
    }

    public void ClickMe()
    {
        if (ItemInhaler.inhaling)
            return;
        SanctumSelectedPanel.Instance.SetMeFull(magicItem);
    }
}
