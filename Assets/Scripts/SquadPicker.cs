using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadPicker : MonoBehaviour
{
    public List<Squad> availavleSquads;
    public Transform[] rowParents;

    [SerializeField]
    GameObject portraitPrefab;

    private void Start()
    {
        gameObject.SetActive(false);
        //availavleSquads = PartyMaster.Instance.squads;

    }

    public void OnEnable()
    {
        if (PartyMaster.Instance.squads == null || PartyMaster.Instance.squads.Count == 0)
            return;
        //int i = 0;
        for(int k = 0; k < PartyMaster.Instance.squads.Count; k++)
        {
            for (int i = 0; i < PartyMaster.Instance.squads[k].pawns.Count; i++)
            {
                GameObject go = Instantiate(portraitPrefab, rowParents[k].transform);
                go.transform.GetChild(0).GetComponentInChildren<Image>().sprite = PartyMaster.Instance.squads[k].pawns[i].PortraitSprite;
            }
        }
    }
}
