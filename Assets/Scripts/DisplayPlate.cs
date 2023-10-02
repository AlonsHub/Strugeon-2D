using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayPlate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TurnTaker turnTaker => turnInfo.GetTurnTaker;
    public TurnInfo turnInfo;

    //Prefab Refs 

    /// <summary>
    /// The main image, usually displays character portraits
    /// </summary>
    [SerializeField]
    Image portrait;
    [SerializeField, Tooltip("This image is a default sprite at aplha 0, by default")]
    Image portraitOverlay;


    [SerializeField]
    private Sprite startPinSprite;

    /// <summary>
    /// Collection of symbols for info such as: status-effect icons, special ability markers (and their cooldown) etc...
    /// </summary>
    [SerializeField]
    List<Image> symbols;

    ///// <summary>
    ///// NOT A PREFAB! this is a reference to the child displayPlate 
    ///// </summary>
    //GameObject _doubleDisplayPlate;

    bool _isCurrent;

    public void Init(TurnInfo ti)
    {
        portrait.color = new Color(1, 1, 1, 1);

        if (ti.isStartPin)
        {
            portrait.sprite = startPinSprite;
            
            //set alpha 0

            return;
        }
            //return;
        turnInfo = ti;
        //turnTaker = ti.GetTurnTaker;
        RefreshDisplay();
    }
    /// <summary>
    /// START PIN
    /// </summary>
    public void Init(Sprite s)
    {
        portrait.sprite = s;
    }

    public void SetAsCurrentStatus(bool isCurrentTurn)
    {
        if (_isCurrent)
            return;

        _isCurrent = isCurrentTurn;
        //temp
        transform.localScale = _isCurrent ? Vector3.one * 1.5f : Vector3.one;
    }

    public void RefreshDisplay()
    {
        portrait.sprite = turnTaker.PortraitSprite;
        
        //Special Ability check, if relevant

        //Symbols and effects, if relevant
        //Color toSet colour

        //if (turnInfo.DoSkipTurn) // probably needs to be a switch 
        //{
        //    toSet = SturgeonColours.Instance.skipGrey;
        //}

        SetPortraitOverlayColour(turnInfo.colour);
    }
    /// <summary>
    /// 
    /// </summary>
    public void RemoveSprite()
    {
        portrait.sprite = null;
        portrait.color = new Color(0, 0, 0, 0);
    }
    /// <summary>
    /// Sets a colour overlay with alpha over the portrait.
    /// E.g. for skipping turns - a black colour will be set over the portrait
    /// </summary>
    /// <param name="col"></param>
    public void SetPortraitOverlayColour(Color col)
    {
        portraitOverlay.color = col;
        //portraitOverlay.= col;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!turnInfo.isStartPin)
        Hover_HpBar.Instance.SetOff();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!turnInfo.isStartPin)
        Hover_HpBar.Instance.SetMe(turnTaker as Pawn);
    }
}
