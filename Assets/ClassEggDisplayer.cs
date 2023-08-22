using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassEggDisplayer : BasicDisplayer
{
    public void SetMeFully(List<Color> cols, List<string> textsPerTextBox, List<Sprite> spritesPerImage)
    {
        if(!base.SetMe(textsPerTextBox, spritesPerImage))
        {
            Debug.LogError("ClassEgg's basic displayer fucked up");
            return;
        }
        for (int i = 0; i < cols.Count; i++)
        {
            if (!images[i])
            {
                Debug.LogError("ClassEggDisplayer has a colour-image count mismatch");
                return;
            }
            images[i].color = cols[i];
        }
    }
}
