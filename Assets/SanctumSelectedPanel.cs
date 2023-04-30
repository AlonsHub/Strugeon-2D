using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumSelectedPanel : BasicDisplayer
{
    public static SanctumSelectedPanel Instance;

    //displayer
    //BasicDisplayer basicDisplayer;

    public MagicItem magicItem;
    [SerializeField]
    MagicItem emptyItem;
    [SerializeField]
    StarGraph starGraphNoolBarPanel;
    private void Awake() //TBF TBD LAZY
    {
        Instance = this;
    }

    public void SetMeFull(MagicItem item)
    {
        magicItem = item;
        starGraphNoolBarPanel.SetToItem(item);
        base.SetMe(new List<string> { item.magicItemName, item._EquipSlotType().ToString(), item.ItemDescription(),item.goldValue.ToString()}, new List<Sprite> {item.itemSprite});
    }
    public void SetMeFull()
    {
        magicItem = null;
        starGraphNoolBarPanel.SetToItem(emptyItem);
        base.SetMe(new List<string> { emptyItem.magicItemName, emptyItem._EquipSlotType().ToString(), emptyItem.ItemDescription(), emptyItem.goldValue.ToString()}, new List<Sprite> { emptyItem.itemSprite});
    }

}
