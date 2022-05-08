using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercPoolDisplayer : MonoBehaviour
{
    //describes a manager which generates a customizeable batch of Displayers(with or without buttons), Instantitates them
    //under an auto-self-sorting partent (layout-groups, grids, whatevers).

    //this can be useful if we want to display all mercs (also: sort and filter for relevance)
    //Instantiate all under an auto-self-sorting element (grid, layout group, etc..)
    //and add generic things to their button's actions, if needed

    //this can recieve prefabs for the type of displayer to instantiate, if just modifying the button action isn't enough 

    [SerializeField] GameObject prefab;

    [SerializeField] Transform gridParent;

    List<GameObject> displayers = new List<GameObject>();

    [SerializeField] MercGearDisplayer mercGearDisplayer;

    List<MercSheet> relevant;
    private void OnEnable()
    {
        //int diff = displayers.Count - PlayerDataMaster.Instance.currentPlayerData.mercSheets.Count;
         relevant = PlayerDataMaster.Instance.GetMercSheetsByAssignments(new List<MercAssignment> { MercAssignment.Available, MercAssignment.Room });
        int diff = displayers.Count - relevant.Count;
        if (diff<0)
        {
            for (int i = 0; i < diff*-1; i++) //is lower than 0
            {
                displayers.Add(Instantiate(prefab, gridParent));
            }
        }
        else if(diff>0)
        {
            for (int i = displayers.Count-1; i >= displayers.Count-diff; i--)
            {
                GameObject temp = displayers[i];
                displayers.RemoveAt(i);
                Destroy(temp);
            }
        }
        //check that they are equal now!
        diff = displayers.Count - relevant.Count;

        if(diff !=0)
        {
            Debug.LogError($"diff ={diff}");
        }

        for (int i = 0; i < displayers.Count; i++)
        {
            LobbyMercDisplayer lobbyMercDisplayer = displayers[i].GetComponent<LobbyMercDisplayer>();
            lobbyMercDisplayer.SetMeFull(relevant[i], this);




            //EquipMercRosterSlot slot = displayers[i].GetComponent<BasicDisplayer>() as EquipMercRosterSlot;
            //slot.index = i;
            //slot.mercPoolDisplayer = this;
            //displayers[i].GetComponent<Button>().onClick.AddListener(() => OpenGearDisplayerByMercIndex(i)); 
        }
       
    }

    public void OpenGearDisplayerByMercIndex(int i)
    {
        //mercGearDisplayer.SetMeFully(PlayerDataMaster.Instance.GetMercSheetByIndex(i));
        mercGearDisplayer.SetMeFully(relevant[i]);
    }
    public void ShowMercOnGearDisplayer(MercSheet mercSheet)
    {
        mercGearDisplayer.SetMeFully(mercSheet);
    }

}
