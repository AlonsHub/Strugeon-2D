using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CallNewSave : MonoBehaviour
{
    public TMP_InputField inputField;

    public void Call()
    {
        PlayerDataMaster.Instance.CreateNewSave(inputField.text);
    }
}
