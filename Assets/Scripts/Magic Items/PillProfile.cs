using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Pill
{
    public static float MaxPotential = 8;

    public NoolColour colour;
    public float potential;
    public float PotentialAsFraction => potential / MaxPotential;

    public Pill(NoolColour noolColour, float pot)
    {
        colour = noolColour;
        potential = pot;
    }

}
[System.Serializable]
public class Nool
{
    public NoolColour colour;
    public float currentValue;
    public float capacity;
    public float regenRate;

    public Nool(NoolColour col, float cap, float regen) //regen should be calculated, IMO TBD
    {
        colour = col;
        capacity = cap;
        currentValue = cap / 2;
        regenRate = regen;
    }

    public void Regen()
    {
        currentValue += regenRate;
        currentValue = Mathf.Clamp(currentValue, 0, capacity);
    }
}

[System.Serializable]
public class PillProfile 
{
    public Pill[] pills;
    public NoolColour dominant;
    int noolsLength = System.Enum.GetValues(typeof(NoolColour)).Length;

    public PillProfile(float[] potentialValuesInOrder) //for New Game (Ashan)
    {
        pills = new Pill[noolsLength];
        for (int i = 0; i < noolsLength; i++)
        {
            pills[i] = new Pill((NoolColour)i, potentialValuesInOrder[i]);
        }
    }

    public List<string> AsStringsData()
    {
        List<string> toReturn = new List<string>();
        foreach (var item in pills)
        {
            toReturn.Add(item.potential.ToString());
        }
        return  toReturn;
    }
     public string AsStringData()
    {
        string toReturn = "";
        foreach (var item in pills)
        {
            toReturn += $"{item.potential}_";
        }
        return toReturn;
    }

}

[System.Serializable]
public class NoolProfile 
{
    public PillProfile pillProfile;

    public Nool[] nools;
    int noolsLength = System.Enum.GetValues(typeof(NoolColour)).Length;
    public System.Action OnAnyValueChanged;

    //basic setters for NewGame and such:
    public NoolProfile( float[] capacityValuesInOrder, PillProfile pp) //for New Game (Ashan)
    {
        pillProfile = pp;
        nools = new Nool[noolsLength];
        for (int i = 0; i < noolsLength; i++)
        {
            //nools[i] = new Nool((NoolColour)i, capacityValuesInOrder[i], GameStats.startingFlatRegenRate);
            nools[i] = new Nool((NoolColour)i, capacityValuesInOrder[i], pillProfile.pills[i].potential*2);
        }
    }


    public void ModifyCurrentValue(NoolColour s, float amount)
    {
        nools[(int)s].currentValue+= amount;
        OnAnyValueChanged?.Invoke();
    }

    public void RegenAll()
    {
        Debug.LogError("regen all");
        foreach (var item in nools)
        {
            if (item.colour == NoolColour.White)
                continue;
            item.Regen();
        }
        OnAnyValueChanged?.Invoke();
    }
    //Some setter that affects all regen values

    //Getters
   

    //overrides
    
}