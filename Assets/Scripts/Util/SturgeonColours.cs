using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SturgeonColours : MonoBehaviour
{
    public static SturgeonColours Instance;

    public Color skipGrey;
    public Color pink;

    //public Color[] Nools;

    public Color noolRed;
    public Color noolBlue;
    public Color noolYellow;
    public Color noolPurple;
    

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
