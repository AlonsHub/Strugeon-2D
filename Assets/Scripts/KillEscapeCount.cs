using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEscapeCount : MonoBehaviour
{
    public Transform escapeColumn;
    public Transform deadColumn;

    public static KillEscapeCount Instance;

    private void Awake()
    {
        Instance = this;
    }
}
