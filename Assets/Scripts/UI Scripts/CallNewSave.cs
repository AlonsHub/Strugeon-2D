using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CallNewSave : MonoBehaviour
{
    public TMP_InputField inputField;

    public void CallSaveAndStartGame()
    {
        PlayerDataMaster.Instance.CreateNewSave(inputField.text);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // 1 - index of "Overworld_Map" scene
    }
}
