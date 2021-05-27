using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtter : MonoBehaviour
{
    public Transform tgt;
    void Update()
    {
        if (tgt)
            transform.LookAt(tgt);
    }
}
