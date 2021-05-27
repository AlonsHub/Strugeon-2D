using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ActionIcon { Attack, Heal, Walk, Censer };
public class BatllelogVerticalGroup : MonoBehaviour
{
    public static BatllelogVerticalGroup Instance;

    public List<Transform> children;
    public float entrySize; //default 100

    public GameObject entryPrefab;

    public Dictionary<ActionIcon, Sprite> actionIconToSprite;
    [SerializeField]
    List<Sprite> actionSprites;

    [SerializeField]
    private float maxChildren = 8;

    private void Awake()
    {
        Instance = this;
        children = new List<Transform>();
        actionIconToSprite = new Dictionary<ActionIcon, Sprite>();
        SetupActionSpriteDictionary();
    }

    public void AddEntry(string actingPawn, ActionIcon actionIcon, string passivePawn)
    {
        foreach (Transform t in children)
        {
            t.localPosition -= Vector3.up * entrySize;
        }

        GameObject go = Instantiate(entryPrefab, transform);
        Rect rect1 = GetComponent<RectTransform>().rect;
        Rect rect2 = go.GetComponent<RectTransform>().rect;
        go.transform.localPosition = new Vector3(0, rect1.height / 2 - rect2.height/2, 0);
        BatllelogEntry be = go.GetComponent<BatllelogEntry>();

        be.Init(actingPawn, actionIconToSprite[actionIcon], passivePawn);
        children.Add(go.transform);

        //while(children.Count >= maxChildren)
        //{
        //    GameObject child = children[children.Count - 1].gameObject;
        //    children.RemoveAt(children.Count - 1);
        //    Destroy(child);
        //}
    }

    void SetupActionSpriteDictionary()
    {
        for (int i = 0; i < Enum.GetNames(typeof(ActionIcon)).Length; i++)
        {
            actionIconToSprite.Add((ActionIcon)i, actionSprites[i]);
        }
    }
}
