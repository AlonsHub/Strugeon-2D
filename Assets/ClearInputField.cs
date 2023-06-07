using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearInputField : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_InputField inputField;
    [SerializeField]
    UnityEngine.UI.Button button;

    private void Start()
    {
        button.interactable = !string.IsNullOrEmpty( inputField.text);

    }
    public void OnFieldValueChanged() //refernced in the InputField's OnValueChanged unityevent (inspector)
    {
        button.interactable = !string.IsNullOrEmpty( inputField.text);
            
    }
    public void OnClick() //refernced in the Button's OnClick unityevent (inspector)
    {
        inputField.text = "";
    }
}
