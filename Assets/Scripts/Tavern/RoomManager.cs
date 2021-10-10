using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    List<RoomBuildDisplayer> roomDisplayers; //set in inspector

    [SerializeField]
    GameObject notEnoughGoldText;

    public int roomPrice = 5; //default for now

    private void OnEnable()
    {
        if (roomDisplayers == null)
            return;

            for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
            {
                roomDisplayers[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);//send squad here?
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
        Room r = new Room();
        PlayerDataMaster.Instance.currentPlayerData.rooms.Add(r); 

        Tavern.Instance.RefreshRooms();

        if (roomDisplayers == null)
            return;

        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        {
            roomDisplayers[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);//send squad here?
        }
    }
}
