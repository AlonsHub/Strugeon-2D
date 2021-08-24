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
    }

    public void OnEnable()
    {
        if (availavleSquads == null || availavleSquads.Count == 0)
            return;
        //int i = 0;
        for(int k = 0; k < availavleSquads.Count; k++)
        {
            for (int i = 0; i < availavleSquads[k].pawns.Count; i++)
            {
                GameObject go = Instantiate(portraitPrefab, rowParents[k].transform);
                go.GetComponentInChildren<Image>().sprite = availavleSquads[k].pawns[i].PortraitSprite;
            }
        }
    }
}
