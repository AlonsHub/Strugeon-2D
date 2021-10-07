using System.Collections;
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
    Sprite defualtSprite;
    public void SetMe(Room r)
    {
        room = r;
        isOccupied = room.isOccupied;
        if (isOccupied)
        {
            //go red?
            leaderIcon.sprite = r.squad.pawns[0].PortraitSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //edit squad if there is one
        Tavern.Instance.EditSquadMenu(room.squad);

        //unset room
        leaderIcon.sprite = defualtSprite;
        //status text change

    }
}
