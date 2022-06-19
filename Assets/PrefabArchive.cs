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

    //TEMP AF TBF - have a good place for a sprite DB/dict 
    public Sprite healthSprite;
    public Sprite swordSprite;


    //NUL BARS:
    //List of nul colour icons 
    [SerializeField]
    List<Sprite> elementIcons;
    //List of nul colour fill-bars
    [SerializeField]
    List<Sprite> elementFillBars;

    public Sprite ElementIcon(int i) => elementIcons[i];
    public Sprite GetElementFillBar(int i) => elementFillBars[i];


    //end TEMP AF TBF
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
    //public Sprite Get
}
