using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ActionSymbol { Attack, Heal, Walk, Censer, Rock, Death, Escape, Summon, Charm};
public enum PsionActionSymbol {Red, Blue, Yellow, Purple };
public class BattleLogVerticalGroup : MonoBehaviour
{
    public static BattleLogVerticalGroup Instance;

    public List<Transform> children;
    public float entrySize; //default 100

    public GameObject entryPrefab;

    public Dictionary<ActionSymbol, Sprite> actionIconToSprite;
    public Dictionary<PsionActionSymbol, Sprite> psionActionIconToSprite;

    public Color[] colours;

    [SerializeField]
    List<Sprite> actionSprites;
     [SerializeField]
    List<Sprite> psionActionSprites;

    [SerializeField]
    private float maxChildren = 8;

    List<GameObject> preLoadedAddons = new List<GameObject>();

    [SerializeField]
    Scrollbar scrollbar;

    Vector3 startPos;
    private void Awake()
    {
        Instance = this;
        children = new List<Transform>();
        //if(transform.childCount >0)
        //{
            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
        //} //if there's something in there, just add it

        actionIconToSprite = new Dictionary<ActionSymbol, Sprite>();
        psionActionIconToSprite = new Dictionary<PsionActionSymbol, Sprite>();
        SetupActionSpriteDictionary();

        scrollbar.onValueChanged.AddListener((float val) => OnScrollbarTouch(val));
        startPos = transform.localPosition;
    }

    private void PushListDown()
    {
        for (int i = children.Count-1; i >= 0; i--)
        {
            children[i].localPosition -= (Vector3.up * entrySize);

            ////----------------MAY NOT BE REQUIRED! the mask hides just enough but generally speaking, it's good practice to just shut them off)
            ////Otherwise you should maybe find a way to better implement them as parts of a sequence that can move it's memebers down
            ////like a stack or something more clever that will itterate and also handle things on movement/push
            ////---------------- Because of this we need to hook into the onvaluehanged of the scrollbar so we can turn all messages
            ////on when the player touches the scrollbar and needs to see all the relevant messages
            ////-------- may be turn on pockets of messages by relevant distance of the scroll bar
            //if(children.Count - i >= maxChildren)
            //{
            //    children[i].gameObject.SetActive(false);
            //}
            // ------------ DEIMPLIMENTED ATM
        }
    }



    public void OnScrollbarTouch(float val)
    {
        if(children.Count < maxChildren)
        {
            return;
        }
        float maxSize = (children.Count - maxChildren) * entrySize; //basically the entire lengths of all logs, minus the length of 1 full scroll/page
        Vector3 endPos = startPos + Vector3.up * maxSize;

        transform.localPosition = Vector3.Lerp(startPos, endPos, val); 

    }
    //maybe need this?
    //public void OnScrollbarLeave()
    //{
    //    foreach (var c in children)
    //    {
    //        c.gameObject.SetActive(true);
    //    }
    //}

    public void AddEntry(string actingPawn, ActionSymbol actionIcon)
    {
        PushListDown();

        GameObject go = Instantiate(entryPrefab, transform);

        go.transform.localPosition = new Vector3(0, (children[children.Count - 1].localPosition + Vector3.up * entrySize).y, 0);

        BatllelogEntry be = go.GetComponent<BatllelogEntry>();

        be.Init(actingPawn, actionIconToSprite[actionIcon]);
        be.SetMyData = actionIcon.ToString(); //escaped only, I think
        children.Add(go.transform);


        foreach (var item in preLoadedAddons) //should also add to description text
        {
            Instantiate(item, go.transform);
        }

        preLoadedAddons.Clear();

        ScaleScrollHandle();
    }
    public void AddEntry(string actingPawn, ActionSymbol actionIcon, string passivePawn)
    {

        //for (int i = 0; i < children.Count; i++)
        //{
        //    children[i].localPosition -= (Vector3.up * entrySize);
        //}
        PushListDown();

        GameObject go = Instantiate(entryPrefab, transform);

        go.transform.localPosition = new Vector3(0, (children[children.Count-1].localPosition + Vector3.up * entrySize).y, 0);

        BatllelogEntry be = go.GetComponent<BatllelogEntry>();

        be.Init(actingPawn, actionIconToSprite[actionIcon], passivePawn);
        //be.SetMyData = actingPawn + " " + actionIcon.ToString() + " " + passivePawn;
        be.SetMyData = actionIcon.ToString();
        children.Add(go.transform);

        foreach (var item in preLoadedAddons)
        {
            Instantiate(item, go.transform);
        }
        preLoadedAddons.Clear();

        ScaleScrollHandle();
    }
    public void AddEntry(string actingPawn, ActionSymbol actionIcon, string passivePawn, int number, Color colour)
    {
        PushListDown();

        GameObject go = Instantiate(entryPrefab, transform);

        go.transform.localPosition = new Vector3(0, (children[children.Count - 1].localPosition + Vector3.up * entrySize).y, 0);

        BatllelogEntry be = go.GetComponent<BatllelogEntry>();

        be.Init(actingPawn, actionIconToSprite[actionIcon], passivePawn, number, colour);

        //be.SetMyData = actingPawn + " " + actionIcon.ToString() + " " + passivePawn + " for: " + number;
        be.SetMyData = actionIcon.ToString();

        children.Add(go.transform);

        foreach (var item in preLoadedAddons)
        {
            Instantiate(item, go.transform);
        }
        preLoadedAddons.Clear();

        ScaleScrollHandle();
    }
    void ScaleScrollHandle()
    {
        //scrollbar.size = (maxChildren / children.Count < 1) ? maxChildren / children.Count : 1;
        scrollbar.size = Mathf.Clamp(maxChildren / children.Count, .1f, 1f); //amazing
    }


    public void AddToLastEntry(GameObject prefab) //things like bonus damage should have their own designated way of adding information to log entries
    {
        //This should work generically by way of pasting a prefab onto the first transform in children
        //Different add-on types would be placed/slotted at different offsets in the prefab
        //E.g. "Censer Bonus Damage" - appears at the bottom-right corner of regular damage logged
        //     "Added Weapon Effects Damage" - appears at the top-right corener of regular damage logged 
        //     "Added Weapon Effect Icon" - appears at the top-left corener of the regular action-icon logged 
        Instantiate(prefab, children[children.Count-1]);

    }
    public void AddToNextEntry(GameObject prefab) //same idea, but preloads an addition to an entry about to be added
    {
        //This should work generically by way of pasting a prefab onto the first transform in children
        //Different add-on types would be placed/slotted at different offsets in the prefab
        //E.g. "Censer Bonus Damage" - appears at the bottom-right corner of regular damage logged
        //     "Added Weapon Effects Damage" - appears at the top-right corener of regular damage logged 
        //     "Added Weapon Effect Icon" - appears at the top-left corener of the regular action-icon logged 

        ///THIS MUST BE CHECKED FOR BY ALL AddEntry overloads!
        //Instantiate(prefab, children[children.Count-1]);
        preLoadedAddons.Add(prefab);
    }

    public void AddPsionEntry(string tgtPawn, PsionActionSymbol psionActionIcon, Color colour)
    {
        //foreach (Transform t in children)
        //{
        //    t.localPosition -= Vector3.up * entrySize;
        //}
        for (int i = 0; i < children.Count; i++)
        {
            children[i].localPosition -= (Vector3.up * entrySize);
        }

        GameObject go = Instantiate(entryPrefab, transform);

        //Rect rect1 = GetComponent<RectTransform>().rect;
        //Rect rect2 = go.GetComponent<RectTransform>().rect;

        //go.transform.localPosition = new Vector3(0, rect1.height / 2 - rect2.height/2, 0);
        go.transform.localPosition = new Vector3(0, (children[children.Count - 1].localPosition + Vector3.up * entrySize).y, 0);

        BatllelogEntry be = go.GetComponent<BatllelogEntry>();


        

        be.Init(tgtPawn, psionActionSprites[(int)psionActionIcon], colours[(int)psionActionIcon]);
        be.SetMyData = psionActionIcon.ToString();
        children.Add(go.transform);
        //CleanList();
        
    }

    //on addentry there should be a change to the scrollbar's total size

    
    
    //void CleanList()
    //{

    //    //------------- OLD Method TO CLEAN THE LOG -----------------------
    //    //if(children.Count <= maxChildren)
    //    //{
    //    //    return;
    //    //}
    //    //while (children.Count > maxChildren)
    //    //{
    //    //    Transform t = children[0];
    //    //    children.RemoveAt(0);
    //    //    Destroy(t.gameObject);
    //    //}
    //    //------------- OLD Method TO CLEAN THE LOG -----------------------

    //}

    void SetupActionSpriteDictionary()
    {
        for (int i = 0; i < Enum.GetNames(typeof(ActionSymbol)).Length; i++)
        {
            actionIconToSprite.Add((ActionSymbol)i, actionSprites[i]);
        }
        for (int i = 0; i < Enum.GetNames(typeof(PsionActionSymbol)).Length; i++)
        {
            psionActionIconToSprite.Add((PsionActionSymbol)i, psionActionSprites[i]);
        }
    }
}
