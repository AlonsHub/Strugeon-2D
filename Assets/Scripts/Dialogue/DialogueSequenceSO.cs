using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif
public enum EndOfDialogue { Close, FadeToBlack};

[CreateAssetMenu()]
public class DialogueSequenceSO : ScriptableObject
{
    public DialogueUnit[] dialogueUnits;


}

//#if UNITY_EDITOR
//[CustomEditor(typeof(DialogueSequenceSO))]
//public class DialogueUnitInspector : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        DialogueSequenceSO dialogueUnit = (DialogueSequenceSO)target;

//        EditorGUILayout.Space();

        
//    }
//}


//#endif

