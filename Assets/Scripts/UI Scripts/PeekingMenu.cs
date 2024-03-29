﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeekingMenu : MonoBehaviour
{


    public Image sr;

    public bool menuOpen;

    //[SerializeField]
    //Button peekButton;
    //[SerializeField]
    //Button hideButton;


    public Animator anim; //temp set in instpector

    public void HideMenu()
    {
        //StartCoroutine("MenuSlideClose");
        if (!menuOpen)
            return;
        anim.SetTrigger("Close");
        menuOpen = false;
    }
    public void ShowMenu()
    {
        if (menuOpen)
            return;
        //StartCoroutine("MenuSlideOpen");
        anim.SetTrigger("Open");
        menuOpen = true;
    }

    public void ToggleMenu() //idle log uses this in inspector
    {
        
        if (menuOpen)
            anim.SetTrigger("Close");
        else
            anim.SetTrigger("Open");

        menuOpen = !menuOpen;
    }
}
