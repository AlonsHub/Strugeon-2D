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
    [SerializeField]
    TMP_Text roomNumber; //resting, away, etc...
    
    //[SerializeField]
    //TMP_Text capacityText; //resting, away, etc...

    [SerializeField]
    Sprite defualtSprite;
    public void SetMe(Room r, int i)
    {
        room = r;
        room.roomNumber = i;
        if (room.isOccupied)
        {
            //go red?
            //capacityText.text = r.size.ToString();
            leaderIcon.sprite = r.squad.pawns[0].PortraitSprite;
            statusText.text = "Team " + r.squad.pawns[0].Name;
            //perhaps read the Room.statusText instead?
        }
        else
        {
            leaderIcon.sprite = defualtSprite;
            statusText.text = "Vacant";
            //perhaps read the Room.statusText instead?
            //should'nt vacant rooms exist already? just have a room with no squad in it... sounds right?
        }
        isOccupied = room.isOccupied;
        roomNumber.text = (i+1).ToString();
    }

    public void SetStatusText(string newText)
    {
        statusText.text = newText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //SquadRoomDisplayer.Instance.SetMe(room);
        if(room.isOccupied)
        Tavern.Instance.SquadRoomSetup(this);
        //edit squad if there is one
        

    }
    public void EditMe()
    {
        Tavern.Instance.EditSquadMenu(room.squad, room);

        //unset room
        leaderIcon.sprite = defualtSprite;
        isOccupied = room.isOccupied = false;
        room.squad = null;
        //room = null;
        //status text change
        SetStatusText("Editing...");
    }
}
