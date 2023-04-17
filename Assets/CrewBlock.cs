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
    TMP_Text upgradePriceText;

    [SerializeField]
    UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;

    public void SetMeEmpty(Room r)
    {
        squad = null;

        room = r; //Mostly redundant, but in-case this is ever called by any other method (that is not SetMe(Room r))

        crewName.text = "Empty Crew";
        

        for (int i = 0; i < r.size; i++)
        {
            _mercBlocks[i].SetToEmpty();
        }

    }
    public void SetMe(Room r)
    {
        room = r;
        upgradePriceText.text = Prices.UpgradeCrewPrice(room.size).ToString();

        if (r.squad == null || r.squad.pawns.Count ==0)
        {
            //squad = new Squad();
            SetMeEmpty(room);
            return;
        }

        squad = r.squad;

        crewName.text = squad.squadName;

        for (int i = 0; i< r.size; i++)
        {
            //MercBlock mb = Instantiate(mercBlockPrefab, mercLayoutParent).GetComponent<MercBlock>();
            if (i<squad.pawns.Count && squad.pawns[i] != null)
                _mercBlocks[i].SetMe(squad.pawns[i]._mercSheet);
            else
                _mercBlocks[i].SetToEmpty();
        }
    }

    public void TryUpgradeRoom() //called by button in inspector 
    {
        if(!room.TryUpgrade())
        {
            Debug.LogError("failed to upgrade room");
            notEnoughGoldText.gameObject.SetActive(true);
        }

        SetMe(room);
    }

    public void EditCrew()//calleb by CrewBlock, set in the prefabs inspector
    {
        Tavern.Instance.squadBuilder.BetterSetToRoom(room);
    }
}
