//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SanctumSelectedItemDisplayer : BasicDisplayer
//{
//    MagicItem magicItem;

//    [SerializeField]
//    NulBarPanel nulBarPanel;

//    [SerializeField]
//    ItemInhaler itemInhaler;
//    public bool SetMeFull(List<string> textsPerTextBox, List<Sprite> spritesPerImage, MagicItem item)
//    {
//        magicItem = item;

//        //itemInhaler.SelectItem(magicItem);
//        if (magicItem != null)
//            nulBarPanel.SetMe(magicItem.spectrumProfile);
//        else
//            nulBarPanel.SetMe(0f);
//        return base.SetMe(textsPerTextBox, spritesPerImage);
//    }

//    public void SetAllNulBarsTo(float value)
//    {
//        nulBarPanel.SetMe(value);
//    }
//}
