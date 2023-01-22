using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SturgeonColours : MonoBehaviour
{
    public static SturgeonColours Instance;

    public Color skipGrey;

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
