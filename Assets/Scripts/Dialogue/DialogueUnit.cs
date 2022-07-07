using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType {Spoken, Descriptive, Choice};


[System.Serializable]
public class DialogueUnit 
{
    public DialogueType dialogueType;

    public string _text;

    //only relevant for choice-units
    [Tooltip("Only relevant for Choice-units")]
    public List<DialogueChoice> dialogueChoices;


}

