using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumSelectedItemDisplayer : BasicDisplayer
{
    MagicItem magicItem;

    [SerializeField]
    SpectrumDisplayer spectrumDisplayer;
    public bool SetMeFull(List<string> textsPerTextBox, List<Sprite> spritesPerImage, MagicItem item)
    {
        magicItem = item;


        return base.SetMe(textsPerTextBox, spritesPerImage);
    }
}
