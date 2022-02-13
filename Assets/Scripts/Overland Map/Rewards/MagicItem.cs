using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Common, Uncommon, Rare, Epic, Legendary};

[System.Serializable]
public class MagicItem 
{
    public string magicItemName; //BAD IDEA, go for enums and dictionaries? something even better perhaps
    public int goldValue;
    public int NF_Value; // TBD

    public Rarity rarity;

    public Sprite itemSprite; // consider keeping an enum to link with a dictionary

}
