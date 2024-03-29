﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{
    public static List<MercName> startMercNames = new List<MercName>{MercName.Shuki, MercName.Smadi, MercName.Yeho};
    public static int startingGold = 1000;
    public static int expToLevel2 = 100;
    public static int maxHpBonusPerLevel = 10;
    public static int minDmgPerLevel = 2;
    public static int maxDmgPerLevel = 3;

    /// <summary>
    /// Tavern Stats
    /// </summary>
    public static int maxRoomSize = 4;

    public static int ExpThresholdByLevel(int level)
    {
        int start = expToLevel2;
        int pre = 0;
        int threshhold = 0;

        for (int i = 1; i <= level; i++)
        {
            threshhold = pre + start;
            pre = start;
            start = threshhold;
        }
        return threshhold;
    }
    public static Vector2Int ExpThresholdsByLevel(int level)
    {
        int start = expToLevel2;
        int pre = 0;
        int threshhold = 0;
        
        for (int i = 1; i <= level; i++)
        {
            threshhold = pre + start;
            pre = start;
            start = threshhold;
        }
        if(level == 1) //Fibbonachi can't work with an initial 0
        {
            pre = 0; //Sets inital "threshold" to be 0 (since no experience is needed to get from level 0 to 1)
        }

        return new Vector2Int(pre, threshhold);
    }

}
