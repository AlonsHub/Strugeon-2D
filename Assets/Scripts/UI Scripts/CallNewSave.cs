using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CallNewSave : MonoBehaviour
{
    public TMP_InputField inputField;
    [SerializeField]
    UnityEngine.UI.Button acceptNameButton;
    //Temp TBF TBD
    [SerializeField]
    LevelSO[] levelSOs;
    //Temp

    public void InteractiveCheck()
    {
        acceptNameButton.interactable = (inputField.text != "");
    }
    public void CallSaveAndStartGame()
    {
        PlayerDataMaster.Instance.CreateNewSave(inputField.text);
        InitSites();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // 1 - index of "Overworld_Map" scene
    }

    /// <summary>
    /// Sets the "isSet" parameter for all sites to false -> for new saves only!
    /// </summary>
    void InitSites()
    {
        foreach (var item in levelSOs)
        {
            item.levelData.isSet = false;
        }
    }
}
