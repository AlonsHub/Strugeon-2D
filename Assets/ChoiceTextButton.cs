using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChoiceTextButton : MonoBehaviour
{
    [SerializeField]
     TMPro.TMP_Text text;
    [SerializeField]
    public DialogueChoice dialogueChoice;

    public void SetMe(DialogueChoice dc)
    {
        dialogueChoice = dc;
        text.text = dc.text;

        
    }
}
