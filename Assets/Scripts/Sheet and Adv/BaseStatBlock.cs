using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu()]
public class BaseStatBlock : ScriptableObject
{
    public MercName mercName;
    public StatBlock block;
    [ContextMenu("fixname")]
    public void FixName()
    {
        AssetDatabase.RenameAsset($"Assets/All_Scriptables/MercBaseStatBlocks/{name}.asset", mercName.ToString() + "_BaseBlock");
    }
}
