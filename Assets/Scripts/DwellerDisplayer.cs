using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DwellerDisplayer : MonoBehaviour
{
    public Image portrait;

    public void KillMe()
    {
        Destroy(gameObject);
    }
}
