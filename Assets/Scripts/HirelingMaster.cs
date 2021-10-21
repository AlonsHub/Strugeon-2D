﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirelingMaster : MonoBehaviour
{
    [SerializeField]
    Transform idleLogParent;
    [SerializeField]
    GameObject newArrivalPrefab;


    public void PromptNewHireling()
    {
        if(!CheckForArrivals())
        {
            Debug.LogWarning("You have all mercs already");
            return;
        }

        List<MercName> existing = PartyMaster.Instance.AllMercs();
        List<MercName> missing = new List<MercName>();
        for (int i = 1; i < Enum.GetNames(typeof(MercName)).Length; i++) //0 == None
        {
            if(!existing.Contains((MercName)i))
            {
                missing.Add((MercName)i);
            }
        }

        if(missing.Count == 0)
        {
            Debug.LogError("Should never happen");
        }

        int rand = UnityEngine.Random.Range(0, missing.Count); //0 is fine here since it's within the list "missing" not the mercName as int

        HirelingWindow hirelingWindow = Instantiate(newArrivalPrefab, idleLogParent).GetComponent<HirelingWindow>();
        //List<MercName> missing = Enum.(typeof(MercName)).ToList().Intersect(PartyMaster.Instance.AllMercs());
        hirelingWindow.SetMe(missing[rand]);

    }

    bool CheckForArrivals() //returns true if new mercs can arrive
    {
        if (PartyMaster.Instance.AllMercs().Count == Enum.GetNames(typeof(MercName)).Length-1) // minus 1 because MercName has a 0 value of None
        {
            return false;
        }

        return true;
    }
}