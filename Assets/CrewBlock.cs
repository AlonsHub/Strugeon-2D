using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrewBlock : MonoBehaviour
{
    Room room;
    Squad squad;


    [SerializeField]
    TMP_Text crewName;
    //[SerializeField]
    //GameObject mercBlockPrefab;


    [SerializeField]
    List<MercBlock> _mercBlocks;
    [SerializeField]
    GameObject notEnoughGoldText;
    [SerializeField]
    TMP_Text upgradePriceText;

    [SerializeField]
    UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;

    public void SetMeEmpty(Room r)
    {
        squad = null;

        room = r; //Mostly redundant, but in-case this is ever called by any other method (that is not SetMe(Room r))

        crewName.text = "Empty Crew";

        
        upgradePriceText.text = Prices.UpgradeCrewPriceAsText(room.size);
       


        for (int i = 0; i < r.size; i++)
        {
            _mercBlocks[i].SetToEmpty();
        }

    }
    public void SetMe(Room r)
    {
        if(!r.isOccupied)
        {
            SetMeEmpty(r);
            return;
        }

        room = r;
        upgradePriceText.text = Prices.UpgradeCrewPriceAsText(room.size);

        squad = r.squad;

        crewName.text = squad.squadName;

        for (int i = 0; i< r.size; i++)
        {
            //MercBlock mb = Instantiate(mercBlockPrefab, mercLayoutParent).GetComponent<MercBlock>();
            if (i<squad.pawns.Count && squad.pawns[i] != null)
                //_mercBlocks[i].SetMe(squad.pawns[i]._mercSheet);
                _mercBlocks[i].SetMe(PlayerDataMaster.Instance.GetMercSheetByName(squad.pawns[i].mercName)); //get the MS from playerdata, not from pawn prefab
            else
                _mercBlocks[i].SetToEmpty();
        }
    }

    public void TryUpgradeRoom() //called by button in inspector 
    {
        if(!room.TryUpgrade())
        {
            Debug.LogError("failed to upgrade room");
            //notEnoughGoldText.gameObject.SetActive(true);
        }
        upgradePriceText.text = Prices.UpgradeCrewPriceAsText(room.size);

        SetMe(room);
    }

    public void EditCrew()//calleb by CrewBlock, set in the prefabs inspector
    {
        Tavern.Instance.squadBuilder.BetterSetToRoom(room);
    }

    public void RefreshLayout()
    {

    }
}
