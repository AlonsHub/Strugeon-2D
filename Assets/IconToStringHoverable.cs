using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToStringHoverable : Hoverable
{
    Image img;
    private void Start()
    {
        img = GetComponent<Image>();
        SetMyData = img.sprite.name;
    }
}
