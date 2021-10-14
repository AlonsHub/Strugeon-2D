using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    List<RoomBuildDisplayer> roomDisplayers; //set in inspector

    [SerializeField]
    GameObject notEnoughGoldText;

    public int RoomPrice { get => PlayerDataMaster.Instance.RoomCount * Prices.roomBasePrice;} //default for now

    [SerializeField]
    TogglePopout myButton;

    private void OnEnable()
    {
        if (roomDisplayers == null)
            return;

        myButton.Toggle(true);

        for (int i = 0; i < PlayerDataMaster.Instance.RoomCount; i++)
        {
            roomDisplayers[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);//send squad here?
        }
        for (int i = PlayerDataMaster.Instance.RoomCount; i < PlayerDataMaster.Instance.MaxRoomCount; i++)
        {
            roomDisplayers[i].SetBuyPriceText(RoomPrice);//send squad here?
        }

    }
    private void OnDisable()
    {
        myButton.Toggle(false);
    }
    public void TryAddRoom()
    {
        //
        if(!Inventory.Instance.TryRemoveGold(RoomPrice))
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
        RefreshAllRooms();
        //for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        //{
        //    roomDisplayers[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);//send squad here?
        //}
        //for (int i = PlayerDataMaster.Instance.RoomCount; i < PlayerDataMaster.Instance.MaxRoomCount; i++)
        //{
        //    roomDisplayers[i].SetBuyPriceText(RoomPrice);//send squad here?
        //}
    }

    public void RefreshAllRooms()
    {
        for (int i = 0; i < PlayerDataMaster.Instance.RoomCount; i++)
        {
            roomDisplayers[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);//send squad here?
        }
        for (int i = PlayerDataMaster.Instance.RoomCount; i < PlayerDataMaster.Instance.MaxRoomCount; i++)
        {
            roomDisplayers[i].SetBuyPriceText(RoomPrice);//send squad here?
        }
    }
}
