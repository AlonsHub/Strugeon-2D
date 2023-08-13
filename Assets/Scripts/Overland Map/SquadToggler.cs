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
    [SerializeField]
    Toggle[] myToggles;


    [SerializeField]
    SquadSlot[] squadSlots;
    [SerializeField]
    Button sendExpeditionButton;
    //[SerializeField]
    //Sprite offFrame;
    //[SerializeField]
    //Sprite onFrame;

    //on value changed

    private void Awake()
    {
        //myToggles = GetComponentsInChildren<Toggle>();
        toggleGroup = GetComponent<ToggleGroup>();
        if (myToggles == null)
        {
            Debug.LogError("No toggles babbbbyyyyyy");
            return;
        }
        //OnEnable();
        foreach (var t in myToggles)
        {
            t.onValueChanged.AddListener(delegate { PickSquadInteractiveCheck(); });
        }
        RefreshSlots();
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
        
        RefreshSlots();
    }
    private void OnDestroy()
    {
        foreach (var t in myToggles)
        {
            t.onValueChanged.RemoveListener(delegate { PickSquadInteractiveCheck(); });
        }
    }

    //private void OnEnable()
    //{
    //    //toggleGroup.SetAllTogglesOff();
    //    RefreshSlots();
    //}
    void PickSquadInteractiveCheck()
    { 
        sendExpeditionButton.interactable = toggleGroup.AnyTogglesOn();
    }
    public void RefreshSlots()
    {
        int count = 0;

        

        //check available squads
        foreach (var item in squadSlots)
        {
            if (item.isRelevant)
            {
                myToggles[count].interactable = true;
                //myToggles[count].onValueChanged.AddListener(delegate { ValuesChanged(); });
                count++;


            }
        }
        for (int i = count; i < squadSlots.Length; i++)
        {
            myToggles[i].interactable = false;
        }
    }


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
    public void SelectIndex(int toSelect)
    {
        if (squadSlots[toSelect].squad != null && squadSlots[toSelect].squad.pawns != null && squadSlots[toSelect].squad.pawns.Count != 0)
        {
            squadSlots[toSelect].SelectMe();
        }
    }
}
