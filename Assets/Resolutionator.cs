using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolutionator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920,1080, false);
        bool doFullscreen = (Screen.currentResolution.width == 1920 && Screen.currentResolution.height == 1080); //current when in windowed mode returns the desktop resolution! this determines if desktop is at 1920x1080
        Screen.SetResolution(1920, 1080, doFullscreen);
        Destroy(gameObject);
    }
}
 