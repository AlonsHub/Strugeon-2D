using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    List<RoomDisplayer> roomDisplayers; //set in inspector

    [SerializeField]
    GameObject notEnoughGoldText;

    public int roomPrice = 5; //default for now

    private void OnEnable()
    {
        if (roomDisplayers == null)
            return;

            for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
            {
                roomDisplayers[i].SetMe();//send squad here?
            } 
        
    }

    public void TryAddRoom()
    {
        //
        if(!Inventory.Instance.TryRemoveGold(roomPrice))
        {
            notEnoughGoldText.SetActive(true);
            return;
        }

        //PlayerDataMaster.Instance.currentPlayerData.totalSquadRooms += 1;
        PlayerDataMaster.Instance.currentPlayerData.rooms.Add(new Room()); 

        Tavern.Instance.RefreshRooms();
    }
}
