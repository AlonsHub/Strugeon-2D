using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFixer : MonoBehaviour
{
    //TEMP AF
    public Button[] buttons;
    
    public float aplhaHitMinimumThreshold;
    
    void Awake()
    {
        //SO TEMPT OMG
        buttons = GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
            button.image.alphaHitTestMinimumThreshold = aplhaHitMinimumThreshold;
    }

   
}
