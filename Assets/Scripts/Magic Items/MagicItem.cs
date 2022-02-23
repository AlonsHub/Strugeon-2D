using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Common, Uncommon, Rare, Epic, Legendary};

[System.Serializable]
public class MagicItem 
{
    public string magicItemName; //BAD IDEA, go for enums and dictionaries? something even better perhaps
    public int goldValue;
    //public int NF_Value; // TBD
    public MagicSpectrumProfile spectrumProfile;

    public Rarity rarity;
    public int dropRateWeight; //higher -> more likely

    public Sprite itemSprite; // consider keeping an enum to link with a dictionary
    public string spriteName;
    public bool FetchSprite()
    {
        if(itemSprite)
        {
            return true;
        }

        itemSprite = Resources.Load<Sprite>($"ItemSprites/{spriteName}");
        return itemSprite; //if null, is false like in the if statement above
    }

}
