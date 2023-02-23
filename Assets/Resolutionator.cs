using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolutionator : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(1920,1080, false);
        bool doFullscreen = (Screen.currentResolution.width == 1920 && Screen.currentResolution.height == 1080); //current when in windowed mode returns the desktop resolution! this determines if desktop is at 1920x1080
        Screen.SetResolution(1920, 1080, doFullscreen);
        Destroy(gameObject);
    }
}
 