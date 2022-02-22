using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class IdleLog : MonoBehaviour
{
    public static IdleLog Instance;

    public static List<IdleLogOrder> backLog;


    public static void AddToBackLog(IdleLogOrder idleLogOrder)
    {
        if(backLog == null)
        {
            Debug.LogError("IdleLog's backlog is null!");
            return;
        }

        backLog.Add(idleLogOrder);
    }

    [SerializeField]
    GameObject basicEntryPrefab; //relevant only for the OverlandMap Version of the idle log

    [SerializeField]
    Transform logParent;

    PeekingMenu peekingMenu;

    private void Awake()
    {
        if(backLog == null)
        {
            backLog = new List<IdleLogOrder>();
        }
        if(Instance !=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        peekingMenu = GetComponent<PeekingMenu>();

        if(backLog.Count>0)
        {
            foreach (var order in backLog)
            {
                RecieveGenericMessage(order.specificDisplayerPrefab, order.strings, order.sprites);
            }
            if (!peekingMenu.menuOpen)
            {
                peekingMenu.ShowMenu();
            }
        }
        backLog.Clear();
    }
    public void RecieveGenericMessage(GameObject prefabMessage, List<string> _strings, List<Sprite> _sprites)
    {
        BasicDisplayer bd = Instantiate(prefabMessage, logParent).GetComponent<BasicDisplayer>();

        bd.SetMe(_strings, _sprites);
    }

    public void RecieveNewMessage(GameObject messagePrefab, List<string> texts) //THIS SHOULD BE IN THE GENERIC METHOD
    {
        GameObject go = Instantiate(messagePrefab, logParent);

        BasicDisplayer basicMessage = go.GetComponent<BasicDisplayer>();
        //List<TMP_Text> textBoxes = GetComponentsInChildren<TMP_Text>().ToList();

        if(basicMessage.textBoxes.Count != texts.Count)
        {
            Debug.LogError("Bugged out, texts and boxes not alligned");
            return;
        }

        for(int i = 0; i< texts.Count; i++)
        {
            basicMessage.textBoxes[i].text = texts[i];
        }
    }

    //public void RecieveNewMessage(List<string> texts) //THIS SHOULD BE IN THE GENERIC METHOD
    //{
    //    GameObject go = Instantiate(basicEntryPrefab, logParent);

    //    BasicMessage basicMessage = go.GetComponent<BasicMessage>();
    //    //List<TMP_Text> textBoxes = GetComponentsInChildren<TMP_Text>().ToList();

    //    if (basicMessage.textBoxes.Count != texts.Count)
    //    {
    //        Debug.LogError("Bugged out. texts and boxes not alligned");
    //        return;
    //    }

    //    for (int i = 0; i < texts.Count; i++)
    //    {
    //        basicMessage.textBoxes[i].text = texts[i];
    //    }

    //    if(!peekingMenu.menuOpen)
    //    {
    //        peekingMenu.ShowMenu();
    //    }
    //}
    //public void RecieveNewMessage(List<string> texts, List<Sprite> sprites) //THIS SHOULD BE IN THE GENERIC METHOD
    //{
    //    GameObject go = Instantiate(basicEntryPrefab, logParent);

    //    BasicMessage basicMessage = go.GetComponent<BasicMessage>();
    //    //List<TMP_Text> textBoxes = GetComponentsInChildren<TMP_Text>().ToList();

    //    if (basicMessage.textBoxes.Count != texts.Count || basicMessage.images.Count != sprites.Count)
    //    {
    //        Debug.LogError("Bugged out. \"texts and boxes\" or \"images and sprites\" count not alligned");
    //        return;
    //    }

    //    for (int i = 0; i < texts.Count; i++)
    //    {
    //        basicMessage.textBoxes[i].text = texts[i];
    //    }
    //    for (int i = 0; i < sprites.Count; i++)
    //    {
    //        basicMessage.images[i].sprite = sprites[i];
    //    }

    //    if(!peekingMenu.menuOpen)
    //    {
    //        peekingMenu.ShowMenu();
    //    }
    //}
    public void RecieveNewMessageWithSiteRef(List<string> texts, List<Sprite> sprites, SiteButton siteRef) //THIS ASSUMES THERES BUTTON RELATED TO SITE IN THE PREFAB (i.e. go to battle button)
    {
        GameObject go = Instantiate(basicEntryPrefab, logParent);
        go.GetComponentInChildren<Button>().onClick.AddListener(() => siteRef.OnClick());
        BasicDisplayer basicMessage = go.GetComponent<BasicDisplayer>();
        //List<TMP_Text> textBoxes = GetComponentsInChildren<TMP_Text>().ToList();

        if (basicMessage.textBoxes.Count != texts.Count || basicMessage.images.Count != sprites.Count)
        {
            Debug.LogError("Bugged out. \"texts and boxes\" or \"images and sprites\" count not alligned");
            return;
        }

        for (int i = 0; i < texts.Count; i++)
        {
            basicMessage.textBoxes[i].text = texts[i];
        }
        for (int i = 0; i < sprites.Count; i++)
        {
            basicMessage.images[i].sprite = sprites[i];
        }

        if(!peekingMenu.menuOpen)
        {
            peekingMenu.ShowMenu();
        }
    }

}
