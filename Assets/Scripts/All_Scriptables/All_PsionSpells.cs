using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu()]
public class All_PsionSpells : ScriptableObject
{
    //public static All_PsionSpells Instance;

    public List<SpellButton> PsionSpells;

    public List<BasicSpellData> BasicSpellDataBase;

    public BasicSpellData[][] BSDsByNoolColour; //for thresholds

//#if UNITY_EDITOR
//    [SerializeField] BasicSpellData[] blueSpells;

//#endif

    [ContextMenu("Call fix BSD for all")]
    public void CallFixBSD_ForAll()
    {
        foreach (var item in PsionSpells)
        {
            item.CopyDataToBSD();
        }
    }
    [ContextMenu("BSD Database pull")]
    public void BSD_DatabasePull()
    {
        BasicSpellDataBase = new List<BasicSpellData>();
        foreach (var item in PsionSpells)
        {
            BasicSpellDataBase.Add(item.basicSpellData);
        }
    }

    [ContextMenu("GetSpellThresholds")]
    public void GetSpellThresholds()
    {
        int numOfNools = System.Enum.GetValues(typeof(NoolColour)).Length;
        BSDsByNoolColour = new BasicSpellData[numOfNools][];
        for (int i = 0; i < numOfNools; i++)
        {
            BasicSpellData[] specificColourSpells = BasicSpellDataBase.Where(x => x.noolColour == (NoolColour)i).ToArray();

            Array.Sort(specificColourSpells, new BSD_Comparer());

            BSDsByNoolColour[i] = specificColourSpells;
        }
        //blueSpells = BSDsByNoolColour[(int)NoolColour.Blue]; // Checked and confirmed!
    }
    //[ContextMenu("FixAllNames")]
    //public void FixAllNames()
    //{
    //    int numOfNools = System.Enum.GetValues(typeof(NoolColour)).Length;
    //    BSDsByNoolColour = new BasicSpellData[numOfNools][];
    //    for (int i = 0; i < numOfNools; i++)
    //    {
    //        BasicSpellData[] specificColourSpells = BasicSpellDataBase.Where(x => x.noolColour == (NoolColour)i).ToArray();

    //        Array.Sort(specificColourSpells, new BSD_Comparer());

    //        BSDsByNoolColour[i] = specificColourSpells;
    //    }
    //    //blueSpells = BSDsByNoolColour[(int)NoolColour.Blue]; // Checked and confirmed!
    //}

    public BasicSpellData CheckIfThresholdWillBePassed(NoolColour nc, float currentCapacityForThisNool, float amountToBeAdded)
    {
        //Instance = this;
        //find next threshold for this colour:
        int nextTop = 0;
        for (int i = 0; i < BSDsByNoolColour[(int)nc].Length; i++)
        {
            if (currentCapacityForThisNool >= BSDsByNoolColour[(int)nc][i].noolCost)
            {
                nextTop = i + 1;
            }
            else
                break;
        }

        if(nextTop >= BSDsByNoolColour[(int)nc].Length)
            return null;
        if(currentCapacityForThisNool + amountToBeAdded < BSDsByNoolColour[(int)nc][nextTop].noolCost) //ELSE -> amount+capacity will rise over nextTop, so return the BSD
            return null;

        return BSDsByNoolColour[(int)nc][nextTop];
    }
}
