using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
[System.Serializable]
public class DEPRECATED_MercSlot : MonoBehaviour
{
    public Image img;
    public Sprite defaultSprite;
    //public TMP_Text nameText;
    public bool isOccupied = false;
    public bool isPartySlot;
    public Pawn pawn;

    //[SerializeField]
    float doubleClickGraceTime = 1f;
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
        if (isPartySlot)
        {

            //remove from currentParty
            //RefMaster.Instance.selectionScreenDisplayer.partyMercs.Remove(pawn);
            PartyMaster.Instance.currentMercParty.Remove(pawn);
            // RefMaster.Instance.selectionScreenDisplayer.availableMercSlots[RefMaster.Instance.selectionScreenDisplayer.availableMercs.Count].AddMerc(pawn);
            //RefMaster.Instance.selectionScreenDisplayer.availableMercs.Add(pawn);
            PartyMaster.Instance.availableMercPrefabs.Add(pawn);



        }
        else
        {
            if (PartyMaster.Instance.currentMercParty.Count >= 3) //change 3 to PartyMaster.Instance.maxPartyMercs
            {
                Debug.Log("PARTY FULL!");
                return;
            }
            //RefMaster.Instance.selectionScreenDisplayer.availableMercs.Remove(pawn);
            PartyMaster.Instance.availableMercPrefabs.Remove(pawn);
            //RefMaster.Instance.selectionScreenDisplayer.partyMercSlots[PartyMaster.Instance.currentMercParty.Count].AddMerc(pawn);
            //RefMaster.Instance.selectionScreenDisplayer.partyMercs.Add(pawn);
            PartyMaster.Instance.currentMercParty.Add(pawn);


        }
        //ClearSlot();
        RefMaster.Instance.selectionScreenDisplayer.RefreshMercDisplay();

        // RefMaster.Instance.selectionScreenDisplayer.MercOnEnable();
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
        if(!isOccupied)
        {
            Debug.LogWarning("Slot has no Pawn");
            return;
        }

        if (isOneClicked)
        {

            RemoveMerc();
        }
        else
        {
            isOneClicked = true;
            StartCoroutine(nameof(DoubleClickWaiter));
        }
    }
    bool isOneClicked = false;
    IEnumerator DoubleClickWaiter()
    {
        isOneClicked = true;

        yield return new WaitForSeconds(doubleClickGraceTime);
        isOneClicked = false;

        //float count = 0;
        //while (count <= doubleClickGraceTime)
        //{
        //    if()

        //    yield return null;
        //    count += Time.deltaTime;
        //}
    }
}
