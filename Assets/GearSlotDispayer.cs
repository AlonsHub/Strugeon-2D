using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSlotDispayer : BasicDisplayer
{
    public override bool SetMe(List<string> textsPerTextBox, List<Sprite> spritesPerImage)
    {
        return base.SetMe(textsPerTextBox, spritesPerImage);
    }
}
