//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(Character))]
//public class CharacterStatManager : Editor
//{
//    Character character;
//    public override void OnInspectorGUI()
//    {
//        character = (Character)target;

//        character._initiativeScore = EditorGUILayout.IntSlider("Initiative Score", character._initiativeScore, 0, 100);
//        ProgressBar(character._initiativeScore / 100f, "Initiative");
//        base.OnInspectorGUI();
//    }

//    void ProgressBar(float value, string label)
//    {
//        // Get a rect for the progress bar using the same margins as a textfield:
//        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
//        EditorGUI.ProgressBar(rect, value, label);
//        EditorGUILayout.Space();
//    }
//}

