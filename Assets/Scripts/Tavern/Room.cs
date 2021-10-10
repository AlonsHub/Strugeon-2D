using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room 
{
    public int size;
    public int roomNumber;

    public Squad squad;

    public bool isOccupied;

    public Room()
    {
        size = 2;
    }

    public bool TryUpgrade()
    {
        int price = size * 5;

        if (Inventory.Instance.TryRemoveGold(price))
        {
            size++;
            return true;
        }
        return false;
    }
}
