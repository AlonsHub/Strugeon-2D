using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BatllelogEntry : MonoBehaviour
{
    

    public TMP_Text actingPawnNameDisplayer;
    public Image actionImage;
    public TMP_Text passivePawnNameDisplayer;
    
    public string actionDescriptionText; //to be accessed by the displayer

    public void Init(string actingName, Sprite actionSprite, string passiveName)
    {
        actingPawnNameDisplayer.text = actingName;
        actionImage.sprite = actionSprite;
        passivePawnNameDisplayer.text = passiveName;
    }

   
}
