using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicMercSlot : MonoBehaviour, IPointerClickHandler
{
    public Pawn merc;

    [SerializeField]
    Image portait;
    [SerializeField]
    Image bgFrame;

    //should NOT be in the basic form
    [SerializeField]
    public SquadRoomDisplayer squadRoomDisplayer;
    [SerializeField]
    Sprite onSprite;
    [SerializeField]
    Sprite offSprite;
    [SerializeField]
    bool isOccupied;


    public void SetMe(Pawn p)
    {
        merc = p;
        portait.sprite = merc.PortraitSprite;
        portait.color = new Color(1, 1, 1, 1);
        isOccupied = true;
    }
    public void SetMe()
    {
        portait.color = new Color(0, 0, 0, 0);
        isOccupied = false;
    }

    public void FrameToggle(bool turnOn)
    {
        bgFrame.sprite = turnOn ? onSprite : offSprite;
        //if (turnOn)
        //{
        //    bgFrame.sprite = onSprite;
        //}
        //else
        //{
        //    bgFrame.sprite = offSprite;
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            squadRoomDisplayer.ClickedMercSlot(this);
            //bg_img.sprite = onBgSprite;
        }
    }
}
