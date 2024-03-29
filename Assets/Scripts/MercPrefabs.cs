﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum MercName {None, Smadi, Shuki, Yeho, Cheeto, Ezra, Hadas, Tikva};

//make one of these for enemies
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

        if(prefabs.Count !=  Enum.GetNames(typeof(MercName)).Length-1)
        {
            Debug.LogError("Number of prefabs does not match number of merc names in the enum!");
            return;
        }
        //implied else:
        enumToPrefab = new Dictionary<MercName, GameObject>();

        for (int i = 0; i < prefabs.Count; i++)
        {
            enumToPrefab.Add((MercName)i+1, prefabs[i]); //basically the same as a list at this moment,
                                                         //but this way allows us to add and remove options from/to this dict
            Debug.Log(((MercName)i + 1).ToString());
        }

    }

    public GameObject EnumToPrefab(MercName mn)
    {
        return enumToPrefab[mn];
    }
    public Pawn EnumToPawnPrefab(MercName mn)
    {
        return enumToPrefab[mn].GetComponent<Pawn>();
    }
    public T EnumToT<T>(MercName mn)
    {
        Debug.Log(mn.ToString());
        return enumToPrefab[mn].GetComponent<T>();
    }
}
