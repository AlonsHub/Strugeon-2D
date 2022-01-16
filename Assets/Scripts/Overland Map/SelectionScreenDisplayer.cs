using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    //public Sprite[] dwellerPortraits; 
    public GameObject dwellerPortraitPrefab; 
    public Transform dwellerPortraitGroupRoot; 

    public TMP_Text lootText; // string separate strings together 
    public TMP_Text goldText; 
    public TMP_Text durationText;


    public float xOffset;

    private void Start()
    {
        if(RefMaster.Instance != null && RefMaster.Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        RefMaster.Instance.selectionScreenDisplayer = this;
        
        if(PartyMaster.Instance.currentMercParty != null)
        partyMercs = PartyMaster.Instance.currentMercParty;
        //availableMercs = PartyMaster.Instance.availableMercs;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //if (PartyMaster.Instance.currentMercParty != null)
        //    partyMercs = PartyMaster.Instance.currentMercParty;
        StartCoroutine("LateOnEnalbe");
    }

    IEnumerator LateOnEnalbe()
    {
        yield return new WaitForSeconds(.1f);
        if (PartyMaster.Instance.currentMercParty != null)
            partyMercs = PartyMaster.Instance.currentMercParty;
    }

    public  void MercOnEnable()
    {
        RefreshMercDisplay();
    }
    public void RefreshMercDisplay()
    {
        int count1 = 0;
        //availableMercSlots = new MercSlot[availableMercs.Count];

        for (int i = 0; i < availableMercSlots.Length; i++)
        {
            availableMercSlots[i].ClearSlot();
        }
        foreach (var merc in PartyMaster.Instance.availableMercPrefabs)
        {
            availableMercSlots[count1].AddMerc(merc);
            count1++;
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
        //dwellerPortraits = new Sprite[levelData.enemies.Count]; //useless? -- yup

        foreach (Pawn enemy in levelData.enemies)
        {
            dwellerNames.text += enemy.name + ", ";
            GameObject go = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot);
            go.GetComponent<Image>().sprite = enemy.PortraitSprite;
        }
        dwellerNames.text = dwellerNames.text.Remove(dwellerNames.text.Length - 2); //kill the last two chars ", "
        lootText.text = "";
        foreach (Object loot in levelData.rewards)
        {
            lootText.text += loot.name + ", ";
        }
        if (lootText.text.Length > 0)
            lootText.text = lootText.text.Remove(lootText.text.Length - 2); //kill the last two chars ", "

        goldText.text = levelData.goldReward.ToString();

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
