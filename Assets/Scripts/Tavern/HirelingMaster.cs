using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirelingMaster : MonoBehaviour
{
    public static HirelingMaster Instance;

    [SerializeField]
    Transform idleLogParent;
    [SerializeField]
    GameObject newArrivalPrefab;

    [SerializeField]
    Animator anim;

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
        if(!CheckForArrivals())
        {
            Debug.LogWarning("You have all mercs already");
            return;
        }

        //List<MercName> existing = PartyMaster.Instance.AllMercs();
        List<MercName> existing = PlayerDataMaster.Instance.GetMercNamesByAssignments(new List<MercAssignment> {MercAssignment.Available, MercAssignment.AwaySquad, MercAssignment.Hireable, MercAssignment.Room});
        List<MercName> missing = new List<MercName>();
        for (int i = 1; i < Enum.GetNames(typeof(MercName)).Length; i++) //0 == None
        {
            if(!existing.Contains((MercName)i))
            {
                missing.Add((MercName)i);
            }
        }

        if(missing.Count == 0)
        {
            Debug.LogError("Should never happen");
        }

        int rand = UnityEngine.Random.Range(0, missing.Count); //0 is fine here since it's within the list "missing" not the mercName as int

        AddHireableToLog(missing[rand]);

        //^^^ Extracte to this AddHireableToLog
        //HirelingWindow hirelingWindow = Instantiate(newArrivalPrefab, idleLogParent).GetComponent<HirelingWindow>();
        ////List<MercName> missing = Enum.(typeof(MercName)).ToList().Intersect(PartyMaster.Instance.AllMercs());
        //hirelingWindow.SetMe(missing[rand], this);

        //PlayerDataMaster.Instance.AddHireableMerc(missing[rand]); //Kinda sucks...

        ////anim.SetTrigger("Open");
        //idleLogParent.GetComponentInParent<PeekingMenu>().ShowMenu();
        //End of Extraction
    }

    void AddHireableToLog(MercName mn)
    {
        HirelingWindow hirelingWindow = Instantiate(newArrivalPrefab, idleLogParent).GetComponent<HirelingWindow>();
        //List<MercName> missing = Enum.(typeof(MercName)).ToList().Intersect(PartyMaster.Instance.AllMercs());
        hirelingWindow.SetMe(mn, this);

        //PlayerDataMaster.Instance.AddHireableMerc(missing[rand]); //Kinda sucks...
        PlayerDataMaster.Instance.AddHireableMerc(mn); //Doesnt suck now!

        //anim.SetTrigger("Open");
        idleLogParent.GetComponentInParent<PeekingMenu>().ShowMenu();
    }

    public void CloseMe()
    {
        //anim.SetTrigger("Close");
        idleLogParent.GetComponentInParent<PeekingMenu>().HideMenu(); //cache this! TBF
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
