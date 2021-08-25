using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBuilder : MonoBehaviour
{
    [SerializeField]
    Transform availableSlotsParent;
    [SerializeField]
    RosterSlot[] availableSlots;
    [SerializeField]
    Transform partySlotsParent;
    [SerializeField]
    RosterSlot[] partySlots; //limited by Room_Level (int) - from avishy's room logic

    public Squad tempSquad;

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
        PartyMaster.Instance.AddNewSquad(tempSquad.pawns);
    }


}