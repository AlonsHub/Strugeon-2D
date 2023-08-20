using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicSpellData 
{
    public string spellName;
    public string longDescription;
    public string shortDescription;
    public float noolCost;
    public NoolColour noolColour;
    /// <summary>
    /// Set to -1 if infinite?
    /// </summary>
    public int spellDuration; 
    public Sprite icon;
}
