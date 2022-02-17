using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceHorizontalGroup : MonoBehaviour
{
    //public Transform origin;
    //int count = 0;
    [SerializeField]
    float delta; //set in inspector

    [ContextMenu("Update Group")]
    public void UpdateGroup()
    {
        //count = transform.childCount;
        int count =0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                transform.GetChild(i).localPosition = delta * (count) * Vector3.right;
                count++;
            }
        }
    }
}
