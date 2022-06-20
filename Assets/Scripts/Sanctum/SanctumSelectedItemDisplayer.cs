using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumSelectedItemDisplayer : BasicDisplayer
{
    MagicItem magicItem;

    [SerializeField]
    NulBarPanel nulBarPanel;

    [SerializeField]
    ItemInhaler itemInhaler;
    public bool SetMeFull(List<string> textsPerTextBox, List<Sprite> spritesPerImage, MagicItem item)
    {
        magicItem = item;

        itemInhaler.SelectItem(magicItem);

        nulBarPanel.SetMe(magicItem.spectrumProfile);
        return base.SetMe(textsPerTextBox, spritesPerImage);
    }
}
