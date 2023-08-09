using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class MagicItemSO : ScriptableObject
{
    public MagicItem magicItem;

#if UNITY_EDITOR
    [ContextMenu("call fetch")]public void CallFetch()
    {
        magicItem.FetchSprite();
    }
#endif
    //[ContextMenu("call fix")]public void CallFix()
    //{
    //    magicItem.Fix();
    //}

}
