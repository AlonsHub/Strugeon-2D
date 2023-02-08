using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StatusIcon : WorldSpaceHoverable
{
    SpriteRenderer sr; //just in case, if not used - save the string name of the icon's sprite

    //public override string GetMyData => _getMyData();
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        SetMyData = _getMyData();
    }

    string _getMyData()
    {
        //return sr.sprite.name.Replace("Icon", ""); 
        if (sr && sr.sprite)
            return sr.sprite.name;
        else
            return "nothing";
    }
}
