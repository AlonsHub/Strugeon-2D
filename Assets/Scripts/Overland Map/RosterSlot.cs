using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
[System.Serializable]
public class RosterSlot : MonoBehaviour
{
    public Image img;
    [SerializeField]
    Sprite defaultSprite;
    //public TMP_Text nameText;
    public bool isOccupied = false;
    public Pawn pawn;

    public bool isPartySlot;
    public void AddMerc(Pawn p)
    {
        pawn = p;
        img.sprite = pawn.PortraitSprite;
        isOccupied = true;
        //nameText.text = p.Name;
        //if(isPartySlot)
        //{
        //    RefMaster.Instance.selectionScreenDisplayer.availableMercs.Add(pawn);
        //}
        //else
        //{

        //    RefMaster.Instance.selectionScreenDisplayer.partyMercs.Add(pawn);

        //}
    }

    public void RemoveMerc()
    {
        //on click, if a merc exists, move to relevant pool:
        if (isPartySlot)
        {

        }
        else
        {

        }
    }

    public void ClearSlot()
    {
        pawn = null;
        img.sprite = defaultSprite;
        isOccupied = false;
        //nameText.text = "";
    }

    public void OnClick()
    {
        if (!isOccupied)
        {
            Debug.LogWarning("Slot has no Pawn");
            return;
        }

        RemoveMerc();
    }
}
