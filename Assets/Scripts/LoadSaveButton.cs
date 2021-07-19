using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveButton : MonoBehaviour
{
    public PlayerData playerDataToLoad;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadMe);
        //GetComponent<Button>().onClick +=;
    }

    public void LoadMe()
    {

    }
}
