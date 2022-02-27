using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SquadSiteAndTimeOfDeparture
{
    public int squadIndex;
    public LevelEnum siteEnum;
    public string timeOfDeparture;

    public SquadSiteAndTimeOfDeparture(int sIndex, LevelEnum levelEnum, DateTime tOfDeparture)
    {
        squadIndex = sIndex;
        siteEnum = levelEnum;
        timeOfDeparture = tOfDeparture.ToString();
    }
}
