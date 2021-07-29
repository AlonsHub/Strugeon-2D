using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]
    private int _gold;
    [SerializeField]
    TMP_Text goldDisplayer;
    public int Gold {get => PlayerDataMaster.Instance.currentPlayerData.gold; 
                     set => PlayerDataMaster.Instance.currentPlayerData.gold = value; }


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
