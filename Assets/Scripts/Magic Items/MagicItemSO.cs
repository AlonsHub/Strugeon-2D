using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class MagicItemSO : ScriptableObject
{
    public MagicItem magicItem;


    [ContextMenu("call fetch")]public void CallFetch()
    {
        magicItem.FetchSprite();
    }
}
