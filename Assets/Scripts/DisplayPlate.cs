using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlate : MonoBehaviour
{
    //Prefab Refs 

    /// <summary>
    /// The main image, usually displays character portraits
    /// </summary>
    [SerializeField]
    Image portrait;

    /// <summary>
    /// Collection of symbols for info such as: status-effect icons, special ability markers (and their cooldown) etc...
    /// </summary>
    [SerializeField]
    List<Image> symbols;

    public void Init(TurnInfo ti)
    {
        if (ti.isStartPin)
            return;

        
        portrait.sprite = ti.GetTurnTaker.PortraitSprite;
    }

    public void SetAsCurrentStatus(bool isCurrentTurn)
    {
        //temp
        transform.localScale = isCurrentTurn ? Vector3.one * 1.5f : Vector3.one;
    }

    

}
