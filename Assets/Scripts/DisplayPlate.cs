using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlate : MonoBehaviour
{
    public TurnTaker turnTaker;

    //Prefab Refs 

    /// <summary>
    /// The main image, usually displays character portraits
    /// </summary>
    [SerializeField]
    Image portrait;
    [SerializeField, Tooltip("The sprite for this image is Empty by default")]
    Image portraitOverlay;

    /// <summary>
    /// Collection of symbols for info such as: status-effect icons, special ability markers (and their cooldown) etc...
    /// </summary>
    [SerializeField]
    List<Image> symbols;

    public void Init(TurnInfo ti)
    {
        if (ti.isStartPin)
            return;
        turnTaker = ti.GetTurnTaker;
        portrait.sprite = ti.GetTurnTaker.PortraitSprite;
    }

    public void SetAsCurrentStatus(bool isCurrentTurn)
    {
        //temp
        transform.localScale = isCurrentTurn ? Vector3.one * 1.5f : Vector3.one;
    }

    /// <summary>
    /// Sets a colour overlay with alpha over the portrait.
    /// E.g. for skipping turns - a black colour will be set over the portrait
    /// </summary>
    /// <param name="col"></param>
    public void SetPortraitOverlayColour(Color col)
    {

    }
}
