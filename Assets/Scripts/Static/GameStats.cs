using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{

    public static int startingGold = 30;

    /// <summary>
    /// Merc-Related stats:
    /// </summary>
    public static List<MercName> startMercNames = new List<MercName>{MercName.Shuki, MercName.Smadi, MercName.Yeho};

    public static int allMercCount = 7;

    public static int expToLevel2 = 100;
    public static int maxHpBonusPerLevel = 10;
    public static int minDmgPerLevel = 2;
    public static int maxDmgPerLevel = 3;

    /// <summary>
    /// Tavern Stats
    /// </summary>
    public static int maxRoomSize = 4;

    //In order of Orange, Yellow, Green, Blue, Red, Purple, Black
    public static float[] startingPsionPotentail = {7,2,3,5,1,6,4};
    //public static float[] startingPsionCapacities = {0,10,0,28,20,45,0,0}; //Psion Level 1 Stats
    public static float[] startingPsionCapacities = {0,47,0,104,39,133,0}; //Psion Level 3 stats
    //public static float startingFlatRegenRate = .2f;

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

    public static int GetNumberFromFibonettaSequence(int level)
    {
        return (level > 3) ? FibonettaSequence(level): 100 * level;
    }

    static int FibonettaSequence(int numberAfterThree)
    {
        int start = 300;
        int pre = 200;
        int threshhold = 0;

        for (int i = 1; i <= numberAfterThree - 3; i++)
        {
            threshhold = pre + start;
            pre = start;
            start = threshhold;
        }
        return threshhold;
    }

    public static Vector2Int GetFibonettaExpThresholdByLevel(int level)
    {
        int pre;
        int threshold = GetNumberFromFibonettaSequence(level);
        if (level == 1) //Fibbonachi can't work with an initial 0
        {
            pre = 0; //Sets inital "threshold" to be 0 (since no experience is needed to get from level 0 to 1)
        }
        else
        {
            pre = GetNumberFromFibonettaSequence(level-1);
        }

        return new Vector2Int(pre, threshold);
    }
}
