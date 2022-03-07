using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToStringHoverable : Hoverable
{
    Image img;
    private void Start()
    {
        if((img = GetComponent<Image>()) && img.sprite != null)
        SetMyData = img.sprite.name;
    }
    //public override string GetMyData => img.sprite.name;
}
