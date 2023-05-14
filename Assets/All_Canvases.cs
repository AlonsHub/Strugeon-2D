using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Canvases : MonoBehaviour
{
    public static Canvas FrontestCanvas; 

    [SerializeField]
    Canvas _FrontestCanvas; 


    void Awake()
    {
        FrontestCanvas = _FrontestCanvas;
    }
}
