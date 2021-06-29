using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionScreenDisplayer : MonoBehaviour
{
    

    //[SerializeField]
    //Level level; //getting deprecated
    LevelData levelData;
    public List<Pawn> availableMercs; // get from PartyManager
    public List<Pawn> partyMercs; //DISPLAYERS - set to PartyManager

    //Displayers:
    public  MercSlot[] availableMercSlots; // Set in inspector
    public MercSlot[] partyMercSlots; // Set in inspector

    public GameObject levelInfoParent; //to Enable/Disable
    public TMP_Text dwellerNames; // string separate strings together 
    public TMP_Text lootText; // string separate strings together 
    public TMP_Text durationText; // string separate strings together 


    public float xOffset;

    private void Start()
    {
        RefMaster.Instance.selectionScreenDisplayer = this;

        partyMercs = PartyMaster.Instance.currentMercParty;
        availableMercs = PartyMaster.Instance.availableMercs;
        gameObject.SetActive(false);
    }

    public  void MercOnEnable()
    {
        RefreshMercDisplay();
    }
    public void RefreshMercDisplay()
    {
        int count1 = 0;
        foreach (var merc in availableMercs)
        {
            availableMercSlots[count1].AddMerc(merc);
            count1++;
        }

        for (int i = count1; i < availableMercSlots.Length; i++)
        {
            availableMercSlots[i].ClearSlot();
        }

        int count2 = 0;
        foreach (var merc in partyMercs)
        {
            partyMercSlots[count2].AddMerc(merc);
            count2++;
        }
        for (int k = count2; k < partyMercSlots.Length; k++)
        {
            partyMercSlots[k].ClearSlot();
        }
    }

    //public void EnableAndSet(Level levelToDisplay)
    //{
    //    gameObject.SetActive(true);
    //    level = levelToDisplay;
        
    //    dwellerNames.text = "";
    //    foreach (Pawn enemy in level.enemies)
    //    {
    //        dwellerNames.text += enemy.name + " ";
    //    }
    //    lootText.text = "";
    //    foreach (Object loot in level.lootList)
    //    {
    //        lootText.text += loot.name + " ";
    //    }

        
    //    levelInfoParent.SetActive(true);
    //    MercOnEnable();
    //}
    public void EnableAndSet(LevelSO levelToDisplay)
    {
        gameObject.SetActive(true);
        levelData = levelToDisplay.levelData;
        dwellerNames.text = "";

        foreach (Pawn enemy in levelData.enemies)
        {
            dwellerNames.text += enemy.name + ", ";
        }
        dwellerNames.text = dwellerNames.text.Remove(dwellerNames.text.Length - 2); //kill the last two chars ", "
        lootText.text = "";
        foreach (Object loot in levelData.rewards)
        {
            lootText.text += loot.name + ", ";
        }
        if (lootText.text.Length > 0)
            lootText.text = lootText.text.Remove(lootText.text.Length - 2); //kill the last two chars ", "


        levelInfoParent.SetActive(true);
        MercOnEnable();
    }
    public void DisableAndReset()
    {
        levelInfoParent.SetActive(false);
       
        gameObject.SetActive(false);
        //level = null;
        levelData = new LevelData(); // empties it
        dwellerNames.text = null;
        lootText.text = null;
    }

    public void Reposition(Transform buttonTrans)
    {
        float x = xOffset;
        if(buttonTrans.position.x >= Screen.width/2)
        {
            x = -xOffset;
            //offset x to left (reduce)
        }
        x += buttonTrans.position.x;
        int y = (int)buttonTrans.position.y;
        Debug.Log(y);
        //y = (int)Mathf.Clamp(y, (float)Screen.height / 4f, (float)Screen.height / 1.25f);
        y = (int)Mathf.Clamp(y, 300 , Screen.height - 300);

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
