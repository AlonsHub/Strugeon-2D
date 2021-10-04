using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadToggler : MonoBehaviour
{
    //public value of chosen squad
    public int selectedToggle = -1; //defualt/no-option-picked = -1
    ToggleGroup toggleGroup;
    //all my toggles
    Toggle[] myToggles;
   

    [SerializeField]
    SquadSlot[] squadSlots;
    //[SerializeField]
    //Sprite offFrame;
    //[SerializeField]
    //Sprite onFrame;

    //on value changed

    private void Awake  ()
    {
        myToggles = GetComponentsInChildren<Toggle>();
        toggleGroup = GetComponent<ToggleGroup>();
        if (myToggles == null)
        {
            Debug.LogError("No toggles babbbbyyyyyy");
            return;
        }
        OnEnable();
        //for (int i = 0; i < myToggles.Length; i++)
        //{
        //    if (squadSlots[i].isRelevant)
        //    {
        //        myToggles[i].onValueChanged.AddListener(delegate { ValuesChanged(); });
        //    }
        //    else
        //    {
        //        myToggles[i].enabled = false;
        //    }
        //}

        //foreach (var t in myToggles)
        //{
        //    t.onValueChanged.AddListener(delegate { ValuesChanged(); });
        //}
    }

    private void OnEnable()
    {

        toggleGroup.SetAllTogglesOff();
        int count = 0;
        //check available squads
        foreach (var item in squadSlots)
        {
            if (item.isRelevant)
            {
                myToggles[count].interactable = true;
                //myToggles[count].onValueChanged.AddListener(delegate { ValuesChanged(); });
                count++;
                Debug.LogError("ONCE");

            }
        }
        for (int i = count; i < squadSlots.Length; i++)
        {
            myToggles[i].interactable = false;
        }
    }

    //public void RefreshSquadSlots()
    //{
    //    foreach (var slot in squadSlots)
    //    {
    //        if(slot.)
    //    }
    //}

    //void ValuesChanged()
    //{
    //    if (selectedToggle != -1)
    //    {
    //        //for (int i = 0; i < PartyMaster.Instance.squads[selectedToggle].pawns.Count; i++)
    //        //{
    //        //    emptyRowParents[selectedToggle].transform.GetChild(i).GetComponent<Image>().sprite = offFrame;
    //        //}
    //        squadSlots[selectedToggle].DeSelectMe();
    //    }

    //    for (int i = 0; i < myToggles.Length; i++)
    //    {
    //        if(myToggles[i].isOn)
    //        {
    //            if(selectedToggle == i)
    //            {
    //                squadSlots[selectedToggle].DeSelectMe();
    //                selectedToggle = -1;
    //                return; // like the "break" two rows below, but also prevents reaching the "SelectMe()"
    //            }
    //            selectedToggle = i;
    //            break;
    //        }
    //    }

    //    //for (int i = 0; i < PartyMaster.Instance.squads[selectedToggle].pawns.Count; i++)
    //    //{
    //    //    emptyRowParents[selectedToggle].transform.GetChild(i).GetComponent<Image>().sprite = onFrame;
    //    //}
    //    squadSlots[selectedToggle].SelectMe();

    //}

    public int SelectedIndex()
    {
        foreach (var slot in squadSlots)
        {
            if(slot.isRelevant && slot.isSelected)
            {
                return slot.index;
            }
        }
        return -1;
    }
}
