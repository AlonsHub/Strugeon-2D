using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestoryer : MonoBehaviour
{
    [SerializeField]
    float TTL;

    private void Start()
    {
        Destroy(gameObject, TTL);
    }
}
