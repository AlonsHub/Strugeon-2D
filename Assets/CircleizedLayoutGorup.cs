using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleizedLayoutGorup : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float angle;
    [SerializeField] Transform centerPoint;

    [SerializeField] List<Transform> toCirclize;

    private void Start()
    {
        Circlize();
    }

    [ContextMenu("Circlize")]
    public void Circlize()
    {
        if (toCirclize == null && toCirclize.Count == 0)
            return;

        for (int i = 0; i < toCirclize.Count; i++)
        {
            if (!toCirclize[i].gameObject.activeSelf)
                continue;
           toCirclize[i].position = centerPoint.position + centerPoint.up * radius;
            centerPoint.Rotate(Vector3.forward * angle);
        }
    }
}
