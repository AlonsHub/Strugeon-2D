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

            DisplayPlate dp = Instantiate(displayerPlatePrefab).GetComponent<DisplayPlate>();
            dp.Init(item);
            AddChild(dp);
            //plates.Add(dp);
        }
        currentPlate = children[0];

        currentPlate.SetAsCurrentStatus(true);
    }

    /// <summary>
    /// Adds a new child to the bottom of the list, and places it accordingly.
    /// </summary>
    /// <param name="displayP"></param>
    public void AddChild(DisplayPlate displayP)
    {
        displayP.transform.parent = transform;
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
        Destroy(displayP.gameObject);
    }
    //public void KillChild(TurnInfo ti)
    //{
    //    //displayP.transform.parent = null;//?
    //    //DisplayPlate dp = children.Where(x => x.)
    //    children.Remove(displayP);
    //    Destroy(displayP.gameObject);
    //}


    public void RefreshAllChildPositions()
    {
        for (int i=0; i< children.Count;i++)
        {
            children[i].transform.localPosition = GetOffsetByIndex(i);
        }
    }
    /// <summary>
    /// Call every time the turnBeltChanges
    /// </summary>
    /// <param name="infos"></param>
    /// <param name="index"></param>
    public void SetAllChildPositions(int index) //infos need to be cached and then MAYBE hooked into via events
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



    //public IEnumerator CycleBeltOnce()
    //{
    //    DisplayPlate trans = RemoveChild(children[0]);
    //    float t = 0f;
    //    AddChild(trans);
    //    while (t*2 <= 1f)
    //    {
    //        for (int i = 0; i < children.Count; i++)
    //        {
    //            children[i].transform.localPosition = LERP(children[i].transform.localPosition, GetOffsetByIndex(i), t*2);
    //        }
    //        yield return new WaitForSeconds(.05f);
    //        t += .05f;
    //    }

    //}

    //Vector3 LERP(Vector2 a, Vector2 b, float t)
    //{
    //    if(t!=0f)
    //    {
    //        //now we know that a is no longer the start point, but we can get the start point - using t
    //        Vector2 endToCurrent = a - b; //goes from b to a, so BACKWARDS!
    //        float length = endToCurrent.magnitude + endToCurrent.magnitude * (1 - t);
    //        a = b + endToCurrent.normalized * length;
    //    }

    //    Vector2 d = b - a;
    //    return a + (t * d);
    //}


    /// <summary>
    /// returns the appropriate offest (under this transform) considering the groups stats (elementSize, spacing and index)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Vector3 GetOffsetByIndex(int index)
    {
        return new Vector3(index * (elementSize + gap), 0,0);
    }

}
