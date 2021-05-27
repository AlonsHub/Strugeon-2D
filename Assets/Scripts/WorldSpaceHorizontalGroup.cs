using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceHorizontalGroup : MonoBehaviour
{
    //public Transform origin;
    public int count = 0;
    float delta = 1.35f; //set in inspector
    public List<Vector3> positions;

    private void Start()
    {
        positions = new List<Vector3>();
    }

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

    public void UpdateGroup()
    {
        count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).localPosition = delta * i * Vector3.right;
        }
    }
}
