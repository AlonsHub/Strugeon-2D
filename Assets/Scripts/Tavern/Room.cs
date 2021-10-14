using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room 
{
    public int size;
    //public int price; //not needed, it's based on how many rooms already exist
    public int roomNumber;

    public Squad squad;

    public bool isOccupied;

    public string statusText;
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

    public void SetStatusText(string newText)
    {
        statusText = newText;
    }
}
