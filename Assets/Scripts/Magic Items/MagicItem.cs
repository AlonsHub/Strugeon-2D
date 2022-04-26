using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { Common, Uncommon, Rare, Epic, Legendary};

[System.Serializable]
public class MagicItem : IEquipable //change name to "ItemData" - unless it kills all the ItemSO for some reason
    //Maybe make this a struct?? TBF TBD FIX NAME ASWELL!
{
    public string magicItemName; //BAD IDEA, go for enums and dictionaries? something even better perhaps
    public int goldValue;
    //public int NF_Value; // TBD
    public MagicSpectrumProfile spectrumProfile;

    public Rarity rarity;
    public int dropRateWeight; //higher -> more likely

    public Sprite itemSprite; // consider keeping an enum to link with a dictionary
    public string spriteName;

    public EquipSlotType fittingSlotType;

    [SerializeField]
    bool isEquipped;
    [SerializeField]
    StatBenefitSO statBenefitSO;

    IBenefit myBenefit => statBenefitSO.statBenefit; //just the single now

    public List<MercClass> relevantClasses;

    public bool FetchSprite()
    {
        //if(itemSprite)
        //{
        //    return true;
        //}
        if (itemSprite.name == spriteName)
        {
            return true;
        }

        itemSprite = Resources.Load<Sprite>($"ItemSprites/{spriteName}");
        return itemSprite; //if null, is false like in the if statement above
    }

    public IBenefit _Benefit()
    {
        return myBenefit;
    }

    public List<IBenefit> _Benefits()
    {
        throw new System.NotImplementedException();
    }

    public string _BenefitsProperNoun()
    {
        if(myBenefit.BenefitProperNoun() ==null)
        return "of Nothing";
        else
        {
            return myBenefit.BenefitProperNoun();
        }
    }

    public string _BenefitsStat()
    {
        if (myBenefit.BenefitProperNoun() == null)
            return "nada?";
        else
        {
            return myBenefit.BenefitStatName();
        }
    }

    public EquipSlotType _EquipSlotType()
    {
        return fittingSlotType;
    }

    public bool _InInventory()
    {
        throw new System.NotImplementedException();
    }

    public bool _IsEquipped()
    {
        return isEquipped;
    }

    public MagicItem _Item()
    {
        return this;
    }
}
