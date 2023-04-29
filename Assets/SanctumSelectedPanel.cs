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
    StarGraphNoolBarPanel starGraphNoolBarPanel;
    private void Awake() //TBF TBD LAZY
    {
        Instance = this;
    }

    public void SetMeFull(MagicItem item)
    {
        magicItem = item;
        starGraphNoolBarPanel.SetToItem(item);
        base.SetMe(new List<string> { item.magicItemName, item._EquipSlotType().ToString() }, new List<Sprite> { item.itemSprite });
    }
}
