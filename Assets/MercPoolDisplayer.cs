using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MercPoolDisplayer : MonoBehaviour, ISearchBarable, ISortableByDropdown
{
    //describes a manager which generates a customizeable batch of Displayers(with or without buttons), Instantitates them
    //under an auto-self-sorting partent (layout-groups, grids, whatevers).

    //this can be useful if we want to display all mercs (also: sort and filter for relevance)
    //Instantiate all under an auto-self-sorting element (grid, layout group, etc..)
    //and add generic things to their button's actions, if needed

    //this can recieve prefabs for the type of displayer to instantiate, if just modifying the button action isn't enough 

    [SerializeField] GameObject prefab;

    [SerializeField] Transform gridParent;

    List<GameObject> onDisplayers;
    List<GameObject> offDisplayers;

    //[SerializeField] MercGearDisplayer mercGearDisplayer;
    List<MercSheet> availableInRoomAndAway;

    List<MercSheet> relevant;

    public static System.Action CallToRefreshPool_PlusSubs;

    private void Awake()
    {
        onDisplayers = new List<GameObject>();
        offDisplayers = new List<GameObject>();
    }
    public void ClearSearch()
    {
        relevant = availableInRoomAndAway;
        DisplayRelevant();
    }

    public void Search(string searchWord)
    {
        relevant = availableInRoomAndAway.Where(x => x.characterName.ToString().IndexOf(searchWord, 0, x.characterName.ToString().Length, System.StringComparison.OrdinalIgnoreCase) != -1).ToList();
        DisplayRelevant();
    }

    private void OnEnable()
    {
        //int diff = displayers.Count - PlayerDataMaster.Instance.currentPlayerData.mercSheets.Count;
        availableInRoomAndAway = PlayerDataMaster.Instance.GetMercSheetsByAssignments(new List<MercAssignment> { MercAssignment.Available, MercAssignment.Room, MercAssignment.AwaySquad });//Excluding Hireable
        relevant = availableInRoomAndAway;                                                                                                                              //relevant = PlayerDataMaster.Instance.currentPlayerData.mercSheets;
        DisplayRelevant();
        CallToRefreshPool_PlusSubs += DisplayRelevant;
    }
    private void OnDisable()
    {
        CallToRefreshPool_PlusSubs -= DisplayRelevant;
    }

    private void DisplayRelevant()
    {
        int diff = onDisplayers.Count - relevant.Count;
        if (diff < 0)
        {
            for (int i = 0; i < diff * -1; i++) //is lower than 0
            {
                if(offDisplayers.Count >0)
                {
                    offDisplayers[offDisplayers.Count - 1].SetActive(true);
                    onDisplayers.Add(offDisplayers[offDisplayers.Count - 1]);
                    offDisplayers.RemoveAt(offDisplayers.Count - 1);
                }
                else
                onDisplayers.Add(Instantiate(prefab, gridParent));
            }
        }
        else if (diff > 0)
        {
            for (int i = onDisplayers.Count- diff; i < onDisplayers.Count; i++)
            {
                offDisplayers.Add(onDisplayers[i]);
                onDisplayers[i].gameObject.SetActive(false);
                //onDisplayers.RemoveAt(i);
                //GameObject temp = onDisplayers[i];
                //onDisplayers.RemoveAt(i);
                //Destroy(temp);
            }
            onDisplayers.RemoveRange(onDisplayers.Count - diff, diff);
        }
        //check that they are equal now!
        diff = onDisplayers.Count - relevant.Count;

        if (diff != 0)
        {
            Debug.LogError($"diff ={diff}");
        }

        for (int i = 0; i < onDisplayers.Count; i++)
        {
            LobbyMercDisplayer lobbyMercDisplayer = onDisplayers[i].GetComponent<LobbyMercDisplayer>();
            lobbyMercDisplayer.SetMeFull(relevant[i], this);
        }
    }
    enum MercLobbySorttingType {Alphabetical, Level, Attack, HP, DateOfAcquistion};
    public void SortThisOut(int typeOfSort, bool lowToHigh)
    {
        switch (typeOfSort)
        {
            case 0:
                if(lowToHigh)
                relevant.Sort(new MercComparer_NameAtoZ());
                else
                relevant.Sort(new MercComparer_NameZtoA());
                break;
            case 1:
                if (lowToHigh)
                    relevant.Sort(new MercComparer_LevelLowToHigh());
                else
                    relevant.Sort(new MercComparer_LevelHighToLow());
                break;
            case 2:
                if (lowToHigh)
                    relevant.Sort(new MercComparer_DamageLowToHigh());
                else
                    relevant.Sort(new MercComparer_DamageHighToLow());
                break;
            case 3:
                if (lowToHigh)
                    relevant.Sort(new MercComparer_HPLowToHigh());
                else
                    relevant.Sort(new MercComparer_HPHighToLow());
                break;
                case 4:
                if (lowToHigh)
                    relevant.Sort(new MercComparer_DateOfAcquisitionEarlyToLate());
                else
                    relevant.Sort(new MercComparer_DateOfAcquisitionLateToEarly());
                break;

            default:
                if (lowToHigh)
                    relevant.Sort(new MercComparer_NameAtoZ());
                else
                    relevant.Sort(new MercComparer_NameZtoA());
                break;
        }
        DisplayRelevant();
    }
}
