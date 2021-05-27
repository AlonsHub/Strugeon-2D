using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefMaster : MonoBehaviour
{
    public static RefMaster Instance;
    public List<Pawn> enemies;
    public List<Pawn> mercs;



    // public SelectionScreenDisplayer selectionScreenDisplayer;

     public Censer censer; //maybe a list?
    //void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //        Destroy(gameObject);
    //    //WAIT A GOD DAMNED MINUTE! THIS IS TERRIBLE!!!!!!!!!
    //    Instance = this;   
    //}
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }


    public void SetEnemyCharacters()
    {
        //enemies = newEnemies;
        foreach(Pawn p in enemies)
        {
            p.Init();
        }
    }
    public void SetMercPawns()
    {
        foreach (Pawn p in mercs)
        {
            p.Init();
        }
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
