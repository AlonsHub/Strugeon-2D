using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{
    public static List<MercName> startMercNames = new List<MercName>{MercName.Shuki, MercName.Smadi, MercName.Yeho};
    public static int startingGold = 100;
    public static int expToLevel2 = 100;
    public static int hpBonusPerLevel = 10;
    public static int minDmgPerLevel = 2;
    public static int maxDmgPerLevel = 3;
}
