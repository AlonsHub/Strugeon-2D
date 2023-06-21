using System.Collections;
using System;
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
    //public ItemSpectrumProfile spectrumProfile;

    public PillProfile pillProfile;
    

    public Rarity rarity;
    

    public Sprite itemSprite; // consider keeping an enum to link with a dictionary
    public string spriteName;

    public EquipSlotType fittingSlotType;

    [SerializeField]
    bool isEquipped;
   
    [SerializeField]
    StatBenefit statBenefit;
    //IBenefit myBenefit => statBenefitSO.statBenefit; //just the single now
    IBenefit myBenefit => statBenefit; //just the single now
    public List<MercClass> relevantClasses;

    public DateTime aquisitionDate;

    //TBF! make a seperate "Description Database" to store all this data.
    //temp, see above ^^^
    [HideInInspector]
    public string ItemDescription() => $"{magicItemName} is a {fittingSlotType} type item, which adds: {ItemBenefitDescription()}"; 
    [HideInInspector]
    public string ItemBenefitDescription() => $"{myBenefit.BenefitStatName()} +{myBenefit.Value()}";

    //end temp
    public MagicItem(string newName, EquipSlotType equipSlotType, StatBenefit benefit, List<MercClass> mercClasses, float[] pots, int gold, string spritename)
    {
        magicItemName = newName;
        fittingSlotType = equipSlotType;
        statBenefit = benefit;
        relevantClasses = mercClasses;
        pillProfile = new PillProfile(pots);
        goldValue = gold;
        spriteName = spritename;

        aquisitionDate = DateTime.Now;
    }
    public List<object> DataAsListOfObjects()
    {

        string classes = "";
        foreach (var item in relevantClasses)
        {
            classes += $"{item}_";
        }
        return new List<object> { magicItemName, ((int)fittingSlotType).ToString(), ((int)statBenefit.statToBenefit).ToString(), myBenefit.Value().ToString(), classes, pillProfile.AsStringData(),  goldValue.ToString(), spriteName };
    }
    public List<string> DataAsListOfStrings()
    {

        string classes = "";
        foreach (var item in relevantClasses)
        {
            classes += $"{item}_";
        }
        return new List<string> { magicItemName, ((int)fittingSlotType).ToString(), ((int)statBenefit.statToBenefit).ToString(), myBenefit.Value().ToString(), classes, pillProfile.AsStringData(),  goldValue.ToString(), spriteName };
    }

    public bool FetchSprite()
    {
        if (itemSprite && itemSprite.name == spriteName) //TBD TBF !!!! TAKE A BETTER LOOK AT THIS LATER!
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
        if(myBenefit.BenefitProperNoun() == null)
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
