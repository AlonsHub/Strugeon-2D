//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DisplayerType {LevelUp, SquadArrival, HirelingArrival};
public class PrefabArchive : MonoBehaviour
{
    //string AllDisplayerTypeNames => System.Enum.GetNames(typeof(DisplayerType)).ToString();
    public static PrefabArchive Instance;
    
    [Tooltip("in this order: LevelUp, SquadArrival, HirelingArrival")]
    public List<GameObject> displayerPrefabs;

    public Sprite bullshit;

    void Awake()
    {
        if(Instance != null&& Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public GameObject GetPrefabByDisplayerType(DisplayerType dt)
    {
        return displayerPrefabs[(int)dt];
    }
    //[ContextMenu("test")]
    //public void TEST()
    //{
    //    IdleLogOrder newOrder = new IdleLogOrder(GetPrefabByDisplayerType(DisplayerType.LevelUp), new List<string> { "something advanced!", $"from" }, bullshit);
    //    IdleLog.AddToBackLog(newOrder, true) ;
    //}
}
