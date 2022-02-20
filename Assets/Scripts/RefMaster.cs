using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefMaster : MonoBehaviour
{
    public static RefMaster Instance;

    public List<Pawn> enemyInstances;
    public List<int> enemyLevels;

    public List<Pawn> mercs;


    //just added 20/06
    public SelectionScreenDisplayer selectionScreenDisplayer;

     public Censer censer; //maybe a list?
   
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

        DontDestroyOnLoad(gameObject);
    }


    //public void SetEnemyCharacters()
    //{
    //    //enemies = newEnemies;
    //    for (int i = 0; i < enemyInstances.Count; i++)
    //    {
    //        SetEnemyLevel(enemyInstances[i], enemyLevels[i]);
    //    }

    //    foreach(Pawn p in enemyInstances)
    //    {
    //        p.Init();
    //    }
    //}
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
        p.maxHP += level * GameStats.maxHpBonusPerLevel;
        p.gameObject.GetComponent<WeaponItem>().ApplySheet(level * GameStats.minDmgPerLevel, level * GameStats.maxDmgPerLevel);
    }
    //public void SetMercPawns(List<Pawn> newMercs)
    //{
    //    mercs = newMercs;
    //    foreach (Pawn p in mercs)
    //    {
    //        p.Init();
    //    }
    //}
    [ContextMenu("Hope")]
    public void DoShit()
    {
        Debug.Log(Instance.name);
    }

    public void SetNewMercList(List<Pawn> newMercs)
    {
        mercs.Clear();
        foreach (var merc in newMercs)
        {
            mercs.Add(merc);
        }
    }

    //public void SetNewPawns(List<Pawn> newPawns, bool isEnemy)
    //{
    //    if (isEnemy)
    //    {
    //        enemies = newPawns;
    //    }
    //    else
    //    {
    //        mercs = newPawns;
    //    }
    //    foreach (Pawn p in newPawns)
    //    {
    //        p.Init();
    //        p.isEnemy = isEnemy;
    //    }
    //}
}
