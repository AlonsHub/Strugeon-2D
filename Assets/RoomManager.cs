using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    List<RoomDisplayer> roomDisplayers;

    private void OnEnable()
    {
        if (roomDisplayers == null)
            return;

      
            for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.totalSquadRooms; i++)
            {
                roomDisplayers[i].SetMe();//send squad here?
            } 
        
    }
}
