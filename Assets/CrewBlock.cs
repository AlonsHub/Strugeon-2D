using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrewBlock : MonoBehaviour
{
    Squad squad;


    [SerializeField]
    TMP_Text crewName;
    //[SerializeField]
    //GameObject mercBlockPrefab;


    [SerializeField]
    List<MercBlock> _mercBlocks;

    public void SetMe()
    {
        squad = null;

        crewName.text = "Empty Crew";
    }
    public void SetMe(Squad s)
    {
        squad = s;

        crewName.text = squad.squadName;
        for (int i = 0; i< squad.pawns.Count; i++)
        {
            //MercBlock mb = Instantiate(mercBlockPrefab, mercLayoutParent).GetComponent<MercBlock>();
            if (squad.pawns[i] != null)
                _mercBlocks[i].SetMe(squad.pawns[i]._mercSheet);
            else
                _mercBlocks[i].SetToEmpty();
        }
    }

}
