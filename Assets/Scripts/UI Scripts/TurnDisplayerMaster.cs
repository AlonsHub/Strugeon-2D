using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDisplayerMaster : MonoBehaviour
{
    [SerializeField]
    GameObject turnDisplayerPrefab;
    [SerializeField]
    GameObject turnDisplayerParent;

    List<TurnDisplayer> allDisplayers;
    int displayerLimit;

    public void Init()
    {
        allDisplayers = new List<TurnDisplayer>();
        for (int i = 0; i < displayerLimit; i++)
        {
            GameObject go = Instantiate(turnDisplayerPrefab, turnDisplayerParent.transform);
            allDisplayers.Add(go.GetComponent<TurnDisplayer>());
        }
        allDisplayers[0].ToggleScale(true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
