﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefMaster : MonoBehaviour
{
    public static RefMaster Instance;

    public List<Pawn> enemyInstances;
    public List<int> enemyLevels;

    public List<Pawn> mercs;


    //just added 20/06 //probably dont need this
    public SelectionScreenDisplayer selectionScreenDisplayer;

     public Censer censer; //maybe a list?

    //logging the dead and cowardly for win/loss window 
    List<MercName> theCowardly;
    List<MercName> theDead;

    //access to cowards and dead lists

    /// <summary>
    /// dont add to me! use AddCoward!
    /// </summary>
    public List<MercName> GetTheCowardly => theCowardly;
    
    /// <summary>
    /// dont add to me! use AddDead!
    /// </summary>
    public List<MercName> GetTheDead => theDead;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            Instance = this;

        if(mercs == null)
        mercs = new List<Pawn>();

        if(enemyInstances == null)
        enemyInstances = new List<Pawn>();

        theDead = new List<MercName>();
        theCowardly = new List<MercName>();

        DontDestroyOnLoad(gameObject);
    }

    public void ClearDeadAndCowards()
    {
        theDead.Clear();
        theCowardly.Clear();
    }

    public void SetEnemyCharacters(List<Pawn> enemList,List<int> levelList)
    {
        enemyInstances = enemList;
        enemyLevels = levelList;

        for (int i = 0; i < enemyInstances.Count; i++)
        {
            SetEnemyLevel(enemyInstances[i], enemyLevels[i]);

            enemyInstances[i].Init();
        }
    }
    public void SetMercPawns()
    {
        foreach (Pawn p in mercs)
        {
            p.Init();
        }
    }

    void SetEnemyLevel(Pawn p, int level)
    {
        p.enemyLevel = level;
        p.maxHP += level * GameStats.maxHpBonusPerLevel;
        p.gameObject.GetComponent<WeaponItem>().ApplySheet(level * GameStats.minDmgPerLevel, level * GameStats.maxDmgPerLevel);
    }
    
    //[ContextMenu("Hope")] //this is sweet, but what the fuck is this even?!
    //public void DoShit()
    //{
    //    Debug.Log(Instance.name);
    //}

    public void SetNewMercList(List<Pawn> newMercs)
    {
        mercs.Clear();
        foreach (var merc in newMercs)
        {
            mercs.Add(merc);
        }
    }


    public void AddCoward(MercName coward)
    {
        theCowardly.Add(coward);
    }
    public void AddDead(MercName dead)
    {
        theDead.Add(dead);
    }

}
