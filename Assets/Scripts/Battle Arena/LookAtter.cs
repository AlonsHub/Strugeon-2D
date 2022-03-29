using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtter : MonoBehaviour
{
    public Transform tgt;
    void FixedUpdate()
    {
        if (tgt)
            transform.LookAt(tgt);
    }

    public void LookOnce(Transform t) //is transform for rotation purposes
    {
        transform.LookAt(t);
    }
}
