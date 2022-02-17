using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveButton : MonoBehaviour
{
    public PlayerData playerDataToLoad;
    //public string saveDir;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadMe);
        //GetComponent<Button>().onClick +=;
    }

    public void LoadMe()
    {
        PlayerDataMaster.Instance.LoadDataFromDisk(playerDataToLoad.playerName);
        Invoke("SlowSceneChange", 1);
    }

    void SlowSceneChange()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
