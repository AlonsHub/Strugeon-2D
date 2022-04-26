using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveButton : MonoBehaviour
{
    public PlayerData playerDataToLoad;
    //public string saveDir;
    public GameObject saveMenuToClose;
    Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadMe);
        button.onClick.AddListener(() => saveMenuToClose.gameObject.SetActive(false));
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
