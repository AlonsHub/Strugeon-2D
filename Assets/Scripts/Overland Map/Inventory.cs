using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField]
    TMP_Text goldDisplayer;
    public int Gold { get => PlayerDataMaster.Instance.currentPlayerData.gold; 
                     private set => PlayerDataMaster.Instance.currentPlayerData.gold = value; } // not sure setter is needed
    [SerializeField]
    GoldUpdater goldUpdater;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {

        goldUpdater = GameObject.Find("Gold Displayer").GetComponent<GoldUpdater>();
        if (!goldUpdater)
        {
            Debug.LogError("no gold updater");
        }
    }

    private void Start()
    {

        goldUpdater = GameObject.Find("Gold Displayer").GetComponent<GoldUpdater>();
        if (!goldUpdater)
        {
            Debug.LogError("no gold updater");
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        goldUpdater.RefreshGold();
    }
    public bool TryRemoveGold(int amount)
    {
        if(amount > Gold)
        {
            //insufficient funds
            return false;
        }

        Gold -= amount;
        goldUpdater.RefreshGold();
        return true;
    }
    

}
