﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RoomButton : MonoBehaviour, IPointerClickHandler
{
    public Room room;
    public bool isOccupied = false;

    [SerializeField]
    Image leaderIcon;
    //Image leaderIcon;
    [SerializeField]
    TMP_Text statusText; //resting, away, etc...
    //[SerializeField]
    //TMP_Text capacityText; //resting, away, etc...

    [SerializeField]
    Sprite defualtSprite;
    public void SetMe(Room r)
    {
        room = r;
        if (room.isOccupied)
        {
            //go red?
            //capacityText.text = r.size.ToString();
            leaderIcon.sprite = r.squad.pawns[0].PortraitSprite;
            statusText.text = "Team " + r.squad.pawns[0].Name;
        }
        else
        {
            leaderIcon.sprite = defualtSprite;
            statusText.text = "Vacant";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //edit squad if there is one
        Tavern.Instance.EditSquadMenu(room.squad, room);

        //unset room
        leaderIcon.sprite = defualtSprite;
        isOccupied = room.isOccupied = false;
        room.squad = null;
        //room = null;
        //status text change

    }
}
