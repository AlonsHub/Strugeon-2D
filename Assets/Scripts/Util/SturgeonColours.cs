using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SturgeonColours : MonoBehaviour
{
    public static SturgeonColours Instance;

    public Color skipGrey;
    public Color pink;
    public Color posionGreen;

    //public Color[] Nools;

    public Color noolOrange;
    public Color noolYellow;
    public Color noolGreen;
    public Color noolBlue;
    public Color noolRed;
    public Color noolPurple;
    public Color noolBlack;
    public Color noolWhite;

    public Color transparentNothing = new Color(0,0,0,0);
    public Color alphaWhite = new Color(1,1,1,1);
    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("More than 1 SturgeonColours!!!!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
