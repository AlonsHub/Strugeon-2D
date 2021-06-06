using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ActionSymbol { Attack, Heal, Walk, Censer, Rock};
public class BattleLogVerticalGroup : MonoBehaviour
{
    public static BattleLogVerticalGroup Instance;

    public List<Transform> children;
    public float entrySize; //default 100

    public GameObject entryPrefab;

    public Dictionary<ActionSymbol, Sprite> actionIconToSprite;
    [SerializeField]
    List<Sprite> actionSprites;

    [SerializeField]
    private float maxChildren = 8;

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
        SetupActionSpriteDictionary();
    }

    public void AddEntry(string actingPawn, ActionSymbol actionIcon, string passivePawn)
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
        go.transform.localPosition = new Vector3(0, (children[children.Count-1].localPosition + Vector3.up * entrySize).y, 0);

        BatllelogEntry be = go.GetComponent<BatllelogEntry>();

        be.Init(actingPawn, actionIconToSprite[actionIcon], passivePawn);
        children.Add(go.transform);
        CleanList();
        //while(children.Count >= maxChildren)
        //{
        //    GameObject child = children[children.Count - 1].gameObject;
        //    children.RemoveAt(children.Count - 1);
        //    Destroy(child);
        //}
    }
    public void AddEntry(string actingPawn, ActionSymbol actionIcon, string passivePawn, int number, Color colour)
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

        be.Init(actingPawn, actionIconToSprite[actionIcon], passivePawn, number, colour);
        children.Add(go.transform);
        CleanList();
        //while(children.Count >= maxChildren)
        //{
        //    GameObject child = children[children.Count - 1].gameObject;
        //    children.RemoveAt(children.Count - 1);
        //    Destroy(child);
        //}
    }

    void CleanList()
    {
        if(children.Count <= maxChildren)
        {
            return;
        }
        while (children.Count > maxChildren)
        {
            Transform t = children[0];
            children.RemoveAt(0);
            Destroy(t.gameObject);
        }
    }

    void SetupActionSpriteDictionary()
    {
        for (int i = 0; i < Enum.GetNames(typeof(ActionSymbol)).Length; i++)
        {
            actionIconToSprite.Add((ActionSymbol)i, actionSprites[i]);
        }
    }
}
