using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadToggler : MonoBehaviour
{
    //public value of chosen squad
    public int selectedToggle = -1; //defualt/no-option-picked = -1

    //all my toggles
    Toggle[] myToggles;

    GameObject[][] portraits = new GameObject[4][]; //this 4 is dependant on number of squads
    
    [SerializeField]
    GameObject[] emptyRowParents;
    [SerializeField]
    Sprite offFrame;
    [SerializeField]
    Sprite onFrame;

    //on value changed

    private void Start()
    {
        myToggles = GetComponentsInChildren<Toggle>();

        if (myToggles == null)
        {
            Debug.LogError("No toggles babbbbyyyyyy");
            return;
        }

        //check available squads

        for (int i = 0; i < myToggles.Length; i++)
        {
            if (i < PartyMaster.Instance.squads.Count)
            {
                myToggles[i].onValueChanged.AddListener(delegate { ValuesChanged(); });
            }
            else
            {
                myToggles[i].gameObject.SetActive(false);
            }
        }

        //foreach (var t in myToggles)
        //{
        //    t.onValueChanged.AddListener(delegate { ValuesChanged(); });
        //}
    }

    void ValuesChanged()
    {
        if (selectedToggle != -1)
        {
            for (int i = 0; i < PartyMaster.Instance.squads[selectedToggle].pawns.Count; i++)
            {
                emptyRowParents[selectedToggle].transform.GetChild(i).GetComponent<Image>().sprite = offFrame;
            }
        }

        for (int i = 0; i < myToggles.Length; i++)
        {
            if(myToggles[i].isOn)
            {
                selectedToggle = i;
                break;
            }
        }

        for (int i = 0; i < PartyMaster.Instance.squads[selectedToggle].pawns.Count; i++)
        {
            emptyRowParents[selectedToggle].transform.GetChild(i).GetComponent<Image>().sprite = onFrame;
        }
    }
}
