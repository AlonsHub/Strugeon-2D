using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prices 
{
    public static int roomBasePrice = 25;
    public static int upgradeRoomBasePrice = 100;

    public static int BaseGPL = 100;
    public static int mercBasePrice = 20;
    /// <summary>
    /// Calculate the GoldPerLevel as a float (not rounded at all)
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static float GPL()
    {
        float runningGPL = BaseGPL;
        for (int i = 1; i < PlayerDataMaster.Instance.currentPlayerData.psionProgressionLevel; i++)
        {
            runningGPL += runningGPL + runningGPL * (2f / 3f);
        }
        return runningGPL;
    }
    public static float GPL(int level)
    {
        float runningGPL = BaseGPL;
        for (int i = 1; i < level; i++)
        {
            runningGPL += runningGPL * (2f / 3f);
        }
        return runningGPL;
    }

    /// <summary>
    /// Int version of GPL - as in GDD
    /// </summary>
    /// <returns></returns>
    public static int ApproximateGPL()
    {
        int temp = Mathf.FloorToInt(GPL());
        if (temp % 10 != 0)
        {
            if (temp % 10 >= 5)
                temp += (10 - (temp % 10)); //adding the remainder needed to raise in the 10s place by and leave the 1s place at 0;
            else
                temp += -(temp % 10); //same, but rounding down. Leaving the 10's place intact
        }

        return temp;
    }
    /// <summary>
    /// Returns MercPrice, adjusted for progression inflation
    /// </summary>
    /// <returns></returns>
    public static int MercPrice()
    {
        //aproximating the rounding to the GDD

        int temp = Mathf.FloorToInt((ApproximateGPL() / 5f) + Mathf.Pow(PlayerDataMaster.Instance.currentPlayerData.psionProgressionLevel, 3));
        if(temp % 10 != 0)
        {
            if (temp % 10 >= 5)
                temp += (10 - (temp % 10)); //adding the remainder needed to raise in the 10s place by and leave the 1s place at 0;
            else
                temp += - (temp % 10); //same, but rounding down. Leaving the 10's place intact
        }

        return temp;
    }


    public static int ApproximateGPL(int level)
    {
        int original = Mathf.FloorToInt(GPL(level));
        int temp = original;
        int count = 0;
        //for (int i = 1; temp > 9 * Mathf.Pow(11, i); i++)
        for (int i = 1; original > 9 * Mathf.Pow(11, i); i++)
        {
            if (temp % 10 != 0)
            {
                //if (temp % 10 >= 5)
                temp += (10 - (temp % 10)); //adding the remainder needed to raise in the 10s place by and leave the 1s place at 0;
                                            //else
                                            //    temp += -(temp % 10); //same, but rounding down. Leaving the 10's place intact
                temp /= 10;
                count++;
            }
        }

        
        return temp * (int)Mathf.Pow(10, count);
    }

    public static int UpgradeCrewPrice(int currentRoomSize)
    {
        return upgradeRoomBasePrice * (currentRoomSize - 1); //2 is the starting amount
    }
    public static int BuyCrewPrice(int currentAmountOfRooms)
    {
        return roomBasePrice * (currentAmountOfRooms); //1 is the starting amount
    }
}
