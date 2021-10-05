using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room 
{
    public int size { get; set; }
    public int roomNumber { get; set; }

    public Squad squad;

    public Room()
    {
        size = 1;
    }
}
