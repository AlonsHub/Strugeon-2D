using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum MercName {Smadi, Shuki, Yeho};

public class MercPrefabs : MonoBehaviour
{
    public static MercPrefabs Instance;

    public List<GameObject> prefabs;


    Dictionary<MercName, GameObject> enumToPrefab;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if(prefabs.Count !=  Enum.GetNames(typeof(MercName)).Length)
        {
            Debug.LogError("Number of prefabs does not match number of merc names in the enum!");
            return;
        }
        //implied else:
        enumToPrefab = new Dictionary<MercName, GameObject>();

        for (int i = 0; i < prefabs.Count; i++)
        {
            enumToPrefab.Add((MercName)i, prefabs[i]); //basically the same as a list at this moment,
                                                       //but this way allows us to add and remove options from/to this dict
        }

    }

    public GameObject EnumToPrefab(MercName mn)
    {
        return enumToPrefab[mn];
    }
}
