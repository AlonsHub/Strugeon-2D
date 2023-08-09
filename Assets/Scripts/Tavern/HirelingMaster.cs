using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirelingMaster : MonoBehaviour
{
    public static HirelingMaster Instance;

    [SerializeField]
    PeekingMenu peekingMenu;

    [SerializeField]
    Transform layoutParent;
    [SerializeField]
    GameObject newArrivalPrefab;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }
    public void LoadExistingHireablesToLog()
    {
        foreach (var item in PlayerDataMaster.Instance.GetMercNamesByAssignment(MercAssignment.Hireable))
        {
            AddHireableToLog(item);
        }
    }
    public void PromptNewHireling()
    {
        if(!PlayerDataMaster.Instance.Victory)
        {
            return;
        }
        PlayerDataMaster.Instance.Victory = false;

        if (!CheckForArrivals())
        {
            Debug.LogWarning("You have all mercs already");
            return;
        }

        if (PlayerDataMaster.Instance.currentPlayerData.wildMercs.Count == 0)
        {
            if(PlayerDataMaster.Instance.currentPlayerData.deadMercCount == GameStats.allMercCount)
            Debug.LogError("Congrats! You just lost Sturgeon");

            return; //to the beginning or to whence you came!
        }


            int rand = UnityEngine.Random.Range(0, PlayerDataMaster.Instance.currentPlayerData.wildMercs.Count); //0 is fine here since it's within the list "missing" not the mercName as int

        AddHireableToLog(PlayerDataMaster.Instance.currentPlayerData.wildMercs[rand]);

    }

    void AddHireableToLog(MercName mn)
    {
        HirelingWindow hirelingWindow = Instantiate(newArrivalPrefab, layoutParent).GetComponent<HirelingWindow>();
        //List<MercName> missing = Enum.(typeof(MercName)).ToList().Intersect(PartyMaster.Instance.AllMercs());
        hirelingWindow.SetMe(mn, this);

        //PlayerDataMaster.Instance.AddHireableMerc(missing[rand]); //Kinda sucks...
        PlayerDataMaster.Instance.AddHireableMerc(mn); //Doesnt suck now!

        //anim.SetTrigger("Open");
        peekingMenu.ShowMenu();
    }

    public void CloseMe()
    {
        //anim.SetTrigger("Close");
        peekingMenu.HideMenu(); //cache this! TBF
    }

    bool CheckForArrivals() //returns true if new mercs can arrive
    {
        //if (PartyMaster.Instance.AllMercs().Count == Enum.GetNames(typeof(MercName)).Length-1) // minus 1 because MercName has a 0 value of None
        if (PlayerDataMaster.Instance.GetMercNamesByAssignments(new List<MercAssignment> { MercAssignment.Available, MercAssignment.AwaySquad, MercAssignment.Hireable, MercAssignment.Room }).Count >= Enum.GetNames(typeof(MercName)).Length - 1) 
        {
            return false;
        }

        return true;
    }
}
