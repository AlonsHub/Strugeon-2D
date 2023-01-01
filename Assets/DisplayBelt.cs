using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBelt : MonoBehaviour
{
    List<DisplayPlate> plates;

    BeltManipulator beltManipulator;
    List<TurnInfo> turnInfos;

    //Prefab Refs
    [SerializeField]
    GameObject displayerPlatePrefab;

    [SerializeField]
    Transform displayerGroup;
    [SerializeField]
    Transform currentTurn;
    DisplayPlate currentPlate;

    /// <summary>
    /// Perform AFTER TurnMachine had completed the Belt
    /// </summary>
    public void Init(BeltManipulator beltMani)
    {
        plates = new List<DisplayPlate>();

        beltManipulator = beltMani;
        turnInfos = beltManipulator.GetTurnInfos();

        foreach (var item in turnInfos)
        {
            if (item.isStartPin)
                continue; //TBA an indicator for the start-pin

            DisplayPlate dp = Instantiate(displayerPlatePrefab, displayerGroup).GetComponent<DisplayPlate>();
            dp.Init(item);
            plates.Add(dp);
        }
        currentPlate = plates[0];

        currentPlate.SetAsCurrentStatus(true);

        beltManipulator.OnNextTurn += NextTurn;
    }

    public void NextTurn()
    {

        ///AAALLLLL BADDDDD!!!!!!! TBF

        currentPlate.SetAsCurrentStatus(false);

        DisplayPlate currentDP = currentPlate;

        plates.Remove(currentDP);
        plates.Add(currentDP);

        currentDP.transform.SetParent(displayerGroup);
        currentDP.transform.SetAsLastSibling(); //This line is intended to push the gameobject down in the hierarchy -> so the horizontal group will take care of positioning.

        currentPlate = plates[0];
        currentPlate.transform.SetParent(currentTurn);
        currentPlate.transform.localPosition = Vector3.zero;
        currentPlate.SetAsCurrentStatus(true); 
                                            

        //itterate over all plate?
    }
    private void OnDisable()
    {
        beltManipulator.OnNextTurn -= NextTurn;

    }
}
