using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleSiteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //SiteData siteData; //TBF depricate this class
    [SerializeField]
    LevelSO levelSO; //a simple LevelSo that should never change

    [SerializeField]
    SquadPicker _squadPicker;
    [SerializeField]
    SiteDisplayer siteDisplayer;

   
    public LevelSO LevelSO { get => levelSO; }
    private void Start()
    {
        if (!_squadPicker)
            _squadPicker = FindObjectOfType<SquadPicker>(); //put those "gets" and "finds" in the   if (!_squadPicker && !(_squadPicker = FindObjectOfType<SquadPicker>()) )
        if (!siteDisplayer)
            siteDisplayer = GetComponentInChildren<SiteDisplayer>();//put those "gets" and "finds" in the   if (!_squadPicker && !(_squadPicker = FindObjectOfType<SquadPicker>()) )
        if (LevelSO && siteDisplayer)
            siteDisplayer.SetMe(this);
    }
    public void OnClick() //set in inspector
    {
        _squadPicker.gameObject.SetActive(true);
        _squadPicker.Refresh(transform); //TBF to also accept offest
        _squadPicker.SetSimpleSite(this);
    }

    public void SendToArena(Squad toSend)
    {
        LevelRef.Instance.SetCurrentLevel(levelSO);
        PartyMaster.Instance.currentSquad = toSend;

        UnityEngine.SceneManagement.SceneManager.LoadScene("ArenaSceneGeneric");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SiteDisplayer.SetActiveToAllInstances(false); //SiteDisplayers should do that on their own
        siteDisplayer.gameObject.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_squadPicker.gameObject.activeInHierarchy)
        siteDisplayer.gameObject.SetActive(false);

    }
}
