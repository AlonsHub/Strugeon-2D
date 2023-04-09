using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewsManager : MonoBehaviour
{
    [SerializeField]
    GameObject crewDisplayBlock;
    [SerializeField]
    Transform gridParent;

    List<CrewBlock> _crewBlocks;

    public void Start()
    {
        _crewBlocks = new List<CrewBlock>();
        foreach (var room in PlayerDataMaster.Instance.currentPlayerData.rooms)
        {
            CrewBlock cb = Instantiate(crewDisplayBlock, gridParent).GetComponent<CrewBlock>();
            cb.SetMe(room.squad);
            _crewBlocks.Add(cb);
        }
    }
}
