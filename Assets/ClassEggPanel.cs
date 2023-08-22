using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassEggPanel : MonoBehaviour
{
    [SerializeField]
    List<GameObject> eggs; //set by class order

    public void SetEggs(List<MercClass> classes)
    {
        for (int i = 0; i < eggs.Count; i++)
        {
            eggs[i].SetActive(classes.Contains((MercClass)i));
        }
    }
}
