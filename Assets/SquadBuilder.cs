using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBuilder : MonoBehaviour
{
    [SerializeField]
    RosterSlot[] availableSlots;
    [SerializeField]
    Transform partySlotsParent;
    [SerializeField]
    RosterSlot[] partySlots; //limited by Room_Level (int) - from avishy's room logic

    public Squad tempSquad;
    [SerializeField]
    MercDataDisplayer mercDataDisplayer;
    //private void Awake()
    //{
    //    //gameObject.SetActive
    //}
    private void OnEnable()
    {

        tempSquad = new Squad();

        //set/instantiate empty party-slots by Room_level

        //print all availables:
        for (int i = 0; i < PartyMaster.Instance.availableMercs.Count; i++)
        {
            availableSlots[i].PopulateSlot(PartyMaster.Instance.availableMercs[i]);
        }
    }

    public void Confirm()
    {
        //PartyMaster.Instance.squads.Add(new Squad(tempSquad.pawns)); //to avoid referencing the tempSquad, which will be cleared soon after this.

        if(tempSquad.pawns.Count > 0)
        PartyMaster.Instance.AddNewSquad(tempSquad.pawns);

        PlayerDataMaster.Instance.GrabAndSaveData();

        UnityEngine.SceneManagement.SceneManager.LoadScene("OverlandMapScene");
    }
    public void SetMercDisplayer(Pawn merc)
    {
        mercDataDisplayer.SetMe(merc);
    }
    public void Refresh()
    {
        for (int i = 0; i < PartyMaster.Instance.availableMercs.Count; i++)
        {
            availableSlots[i].PopulateSlot(PartyMaster.Instance.availableMercs[i]);
        }
        for (int i = PartyMaster.Instance.availableMercs.Count; i < availableSlots.Length; i++)
        {
            availableSlots[i].ClearSlot();
        }

        for (int i = 0; i < tempSquad.pawns.Count; i++)
        {
            partySlots[i].PopulateSlot(tempSquad.pawns[i]);
        }
        for (int i = tempSquad.pawns.Count; i < partySlots.Length; i++)
        {
            partySlots[i].ClearSlot();
        }
    }
}