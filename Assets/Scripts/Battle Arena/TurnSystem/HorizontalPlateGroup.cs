using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HorizontalPlateGroup : MonoBehaviour
{
    //Stats:
    [SerializeField]
    float elementSize;
    [SerializeField]
    float gap;
    [SerializeField]
    float speed; //duration seems like a better choice for this?

    [SerializeField]
    List<DisplayPlate> children;

    public List<DisplayPlate> GetChildren { get => children; }

    List<TurnInfo> infos;
    //List<DisplayPlate> plates;

    //Prefab Refs
    [SerializeField]
    GameObject displayerPlatePrefab;

    [SerializeField]
    DisplayPlate currentPlate;



    private void Awake()
    {
        children = new List<DisplayPlate>();
    }

    public void Init(List<TurnInfo> tis)
    {
        infos = tis;
        
        children = new List<DisplayPlate>();

        foreach (var item in infos)
        {
            if (item.isStartPin)
                continue; //TBA an indicator for the start-pin
            DisplayPlate dp = MakeDisplayPlate(item);
            AddChild(dp);
            //plates.Add(dp);
        }
        currentPlate = children[0];

        currentPlate.SetAsCurrentStatus(true);
    }

    private DisplayPlate MakeDisplayPlate(TurnInfo item)
    {
        DisplayPlate dp = Instantiate(displayerPlatePrefab).GetComponent<DisplayPlate>();
        dp.Init(item);
        RectTransform rectTransform =
        dp.GetComponent<RectTransform>();

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, elementSize);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, elementSize);
        return dp;
    }

    /// <summary>
    /// Adds a new child to the bottom of the list, and places it accordingly.
    /// </summary>
    /// <param name="displayP"></param>
    public void AddChild(DisplayPlate displayP)
    {
        displayP.transform.SetParent( transform);
        children.Add(displayP);

        //manage insertion/shifting of existing members
        displayP.transform.localPosition = GetOffsetByIndex(children.Count);
    }
    public void AddChildByTurnInfo(TurnInfo ti)
    {
        DisplayPlate displayP = MakeDisplayPlate(ti);
        displayP.transform.SetParent(transform);
        children.Add(displayP);

        //manage insertion/shifting of existing members
        displayP.transform.localPosition = GetOffsetByIndex(children.Count);
    }

    ///// <summary>
    ///// MUST RE-PARENT AND REPOSITION
    ///// </summary>
    ///// <param name="displayP"></param>
    //public DisplayPlate RemoveChild(DisplayPlate displayP)
    //{
    //    displayP.transform.parent = null;//?
    //    children.Remove(displayP);
    //    return displayP;
    //}
   
    public void KillChild(DisplayPlate displayP)
    {
        //displayP.transform.parent = null;//?
        children.Remove(displayP);

        SetAllChildPositions();

        Destroy(displayP.gameObject);
    }
    public void KillChild(TurnInfo ti)
    {
        //DisplayPlate dp = children.Where(x => x.turnTaker == ti.GetTurnTaker).SingleOrDefault();
        DisplayPlate dp = children.Where(x => x.turnInfo == ti).FirstOrDefault();
        if (dp == null)
        {
            Debug.LogError("could not find DP");
            return;
        }
        KillChild(dp);
    }

    //public void KillChild(TurnInfo ti)
    //{
    //    //displayP.transform.parent = null;//?
    //    //DisplayPlate dp = children.Where(x => x.)
    //    children.Remove(displayP);
    //    Destroy(displayP.gameObject);
    //}


    //public void RefreshAllChildPositions()
    //{
    //    for (int i=0; i< children.Count;i++)
    //    {
    //        children[i].transform.localPosition = GetOffsetByIndex(i);
    //    }
    //}
    /// <summary>
    /// Call every time the turnBeltChanges
    /// </summary>
    /// <param name="index"></param>
    public void RefreshPortraits(int index) //infos need to be cached and then MAYBE hooked into via events
    {
        //TEST infos and belt sync
        int place = index;
        for (int i = 0; i < infos.Count; i++)
        {
            if (infos[i].isStartPin)
                continue;

            if (place >= infos.Count)
                place = 1;

            children[i - 1].Init(infos[place]);
            place++;
        }  
    }

    void SetAllChildPositions()
    {
        currentPlate = children[0];
        currentPlate.SetAsCurrentStatus(true);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].transform.localPosition = GetOffsetByIndex(i+1);
        }
    }
     

    /// <summary>
    /// returns the appropriate offest (under this transform) considering the groups stats (elementSize, spacing and index)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Vector3 GetOffsetByIndex(int index)
    {
        return new Vector3((index-1) * (elementSize + gap), 0,0);
    }

}
