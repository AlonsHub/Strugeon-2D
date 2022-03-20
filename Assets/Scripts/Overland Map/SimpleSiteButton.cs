using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSiteButton : MonoBehaviour
{
    //SiteData siteData; //TBF depricate this class
    [SerializeField]
    LevelSO levelSO; //a simple LevelSo that should never change

    [SerializeField]
    SquadPicker _squadPicker;

    private void Start()
    {
        if (!_squadPicker)
            _squadPicker = FindObjectOfType<SquadPicker>();
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
}
