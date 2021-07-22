using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveFileDisplayer : MonoBehaviour
{
    public TMP_Text playerName;
    //public string saveFileName; //just the unique part

    public PlayerData playerData; 
   
    public void Init(PlayerData pd)
    {
        //playerName = 
        playerData = pd;

        playerName.text = playerData.playerName;
    }
}
