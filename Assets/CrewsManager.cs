using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewsManager : MonoBehaviour
{
    [SerializeField]
    GameObject crewDisplayBlock;
    [SerializeField]
    Transform gridParent;

    List<CrewBlock> _crewBlocks;

    [SerializeField]
    GameObject notEnoughGoldText;
    [SerializeField]
    TMPro.TMP_Text buyRoomPrice;

    public void OnEnable()
    {
        RefreshCrews();
    }

    private void RefreshCrews()
    {
        if (_crewBlocks == null)
            _crewBlocks = new List<CrewBlock>();

        buyRoomPrice.text = Prices.BuyCrewPrice(PlayerDataMaster.Instance.currentPlayerData.rooms.Count).ToString();

        int delta = _crewBlocks.Count - PlayerDataMaster.Instance.currentPlayerData.rooms.Count;
        if (delta < 0)
        {
            for (int i = 0; i < delta * -1; i++)
            {
                CrewBlock cb = Instantiate(crewDisplayBlock, gridParent).GetComponent<CrewBlock>();
                _crewBlocks.Add(cb);
            }
        }
        else if (delta > 0)
        {
            for (int i = 0; i < delta; i++)
            {
                CrewBlock cb = _crewBlocks[_crewBlocks.Count - 1];
                _crewBlocks.Remove(cb);
                Destroy(cb.gameObject);
            }
        }
        //foreach (var room in PlayerDataMaster.Instance.currentPlayerData.rooms)
        for (int i = 0; i < PlayerDataMaster.Instance.currentPlayerData.rooms.Count; i++)
        {
            _crewBlocks[i].SetMe(PlayerDataMaster.Instance.currentPlayerData.rooms[i]);
        }
    }

    public void TryAddCrew()
    {
        if (!Inventory.Instance.TryRemoveGold(Prices.BuyCrewPrice(PlayerDataMaster.Instance.currentPlayerData.rooms.Count)))
        {
            notEnoughGoldText.SetActive(true);
            return;
        }

        Room r = new Room(PlayerDataMaster.Instance.currentPlayerData.rooms.Count);
        //r.ini
        PlayerDataMaster.Instance.currentPlayerData.rooms.Add(r);

        //Tavern.Instance.RefreshRooms();


        RefreshCrews();
    }

}
