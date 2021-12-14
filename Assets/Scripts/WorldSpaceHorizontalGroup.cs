using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceHorizontalGroup : MonoBehaviour
{
    //public Transform origin;
    //int count = 0;
    [SerializeField]
    float delta; //set in inspector

    [SerializeField]
    Transform startPos;

    //public List<Vector3> positions;

    //private void Start()
    //{
    //    positions = new List<Vector3>();
    //}

    //public void AddEffectIconToGroup(GameObject go)
    //{
    //    count++;
    //    UpdateGroup();

    //}
    //public void RemoveEffectIconToGroup(GameObject go)
    //{
    //    count--;
    //    UpdateGroup();
    //}
    [ContextMenu("Update Group")]
    public void UpdateGroup()
    {
        //count = transform.childCount;
        int count =0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).localPosition = delta * (count) * Vector3.right;
                count++;
            }
        }
    }
}
