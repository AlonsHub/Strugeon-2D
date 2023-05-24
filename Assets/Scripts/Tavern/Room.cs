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

    public RoomButton roomButton;
    public Room(int rNumber)
    {
        size = 2;
        roomNumber = rNumber;
        isOccupied = false;
        squad = new Squad(roomNumber);
        //squad.roomNumber = roomNumber;
        SetStatusText("Vacant");

    }

    public bool TryUpgrade()
    {
        //int price = size * 5;

        if (Inventory.Instance.TryRemoveGold(size * Prices.UpgradeCrewPrice(size)))
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

    public void ClearRoom()
    {
        isOccupied = false;
        squad = null;
        SetStatusText("Vacant");
    }
}
