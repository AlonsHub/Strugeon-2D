using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacntumItemHover : HoverableWithBox
{
    [SerializeField]
    SanctumItemDisplayer sanctumItemDisplayer;
    private void Start()
    {
        MagicItem mi = sanctumItemDisplayer.magicItem;
        SetMyDisplayer(new List<string> { mi.magicItemName, mi.fittingSlotType.ToString(),  mi._Benefit().BenefitStatName(), mi._Benefit().Value().ToString()}, new List<Sprite> { mi.itemSprite});
    }
}
