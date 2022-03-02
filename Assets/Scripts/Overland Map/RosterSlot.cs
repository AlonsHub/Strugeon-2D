using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
[System.Serializable]
public class RosterSlot : MonoBehaviour, IPointerClickHandler
{
    
    public Image img;
    public Image bg_img;
    [SerializeField]
    Sprite defaultPortraitSprite;
    [SerializeField]
    Sprite onBgSprite;
    [SerializeField]
    Sprite offBgSprite;

    //public TMP_Text nameText;
    public bool isOccupied = false;
    public Pawn pawn;

    public bool isPartySlot;

    [SerializeField]
    SquadBuilder squadBuilder;
    [SerializeField]
    GameObject mercDisplayer;

    public SquadBuilder sb;

    public void SetMe(Pawn p)
    {
        pawn = p;
        img.sprite = pawn.PortraitSprite;
        img.color = new Color(1, 1, 1, 1);
        isOccupied = true;
    }
    public void SetMe()
    {
        img.sprite = defaultPortraitSprite;
        img.color = new Color(0,0,0,0);
        isOccupied = false;
    }

    public void RemoveMerc()
    {
        //on click, if a merc exists, move to relevant pool:
        if (isPartySlot)
        {
            squadBuilder.tempSquad.RemoveMerc(pawn);
            PartyMaster.Instance.availableMercPrefabs.Add(pawn);

        }
        else
        {
            squadBuilder.tempSquad.AddMerc(pawn);
            PartyMaster.Instance.availableMercPrefabs.Remove(pawn);
        }
        isOccupied = false;
        FrameToggle(false);
        squadBuilder.Refresh();
    }

    public void ClearSlot()
    {
        pawn = null;
        img.sprite = defaultPortraitSprite;
        img.color = new Color(0, 0, 0, 0);
        bg_img.sprite = offBgSprite;
        isOccupied = false;
        //nameText.text = "";
    }

    //public void OnClick() // THIS MIGHT BE REDUNDANT IF ITS USED IT MAY CAUSE ISSUES!
    //{
    //    if (!isOccupied)
    //    {
    //        Debug.LogWarning("Slot has no Pawn");
    //        return;
    //    }
    //    if (!squadBuilder.mercDataDisplayer.gameObject.activeSelf)
    //        squadBuilder.mercDataDisplayer.gameObject.SetActive(true);
    //    squadBuilder.SetMercDisplayer(pawn);
        
    //    //RemoveMerc();
    //}

    public void OnPointerClick(PointerEventData eventData) // SET BY OCDE
    {
        if (!isOccupied)
        {
            Debug.LogWarning("Slot has no Pawn");
            return;
        }

        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    if(isPartySlot || sb.tempSquad.pawns.Count < sb.ToRoom.size)
        //    RemoveMerc();
        //}
        //else
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    if (!squadBuilder.gameObject.activeSelf)
        //        squadBuilder.gameObject.SetActive(true);
        //    squadBuilder.SetMercDisplayer(pawn);

        //    squadBuilder.TurnAllOff();
        //    FrameToggle(true);
        //    //bg_img.sprite = onBgSprite;
        //}
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //if(isPartySlot || sb.tempSquad.pawns.Count < sb.ToRoom.size)
            //{
            if (isOneClicked)
            {
                if (isPartySlot || sb.tempSquad.pawns.Count < sb.ToRoom.size)
                    RemoveMerc();
            }
            else
            {
                isOneClicked = true;

                if (!squadBuilder.mercDataDisplayer.gameObject.activeInHierarchy)
                    squadBuilder.mercDataDisplayer.gameObject.SetActive(true);
                squadBuilder.SetMercDisplayer(pawn);
                squadBuilder.TurnAllFramesOff();
                FrameToggle(true);

                StartCoroutine(nameof(DoubleClickWaiter));
            }
            //}
            //RemoveMerc();
        }
        //else
        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    if (!squadBuilder.gameObject.activeSelf)
        //        squadBuilder.gameObject.SetActive(true);
        //    squadBuilder.SetMercDisplayer(pawn);

        //    squadBuilder.TurnAllOff();
        //    FrameToggle(true);
        //    //bg_img.sprite = onBgSprite;
        //}
    }

    bool isOneClicked = false;
    float doubleClickGraceTime = 1;
    IEnumerator DoubleClickWaiter()
    {
        isOneClicked = true;

        yield return new WaitForSeconds(doubleClickGraceTime);
        isOneClicked = false;

    }

    public void FrameToggle(bool turnOn)
    {
        if(turnOn)
        {
            bg_img.sprite = onBgSprite;
        }
        else
        {
            bg_img.sprite = offBgSprite;
        }
    }
}
