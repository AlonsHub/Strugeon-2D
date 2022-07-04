using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueWindow : MonoBehaviour
{
    [SerializeField] //drag this! dont GetComponent it ever
    TMP_Text mainText;

    [SerializeField]
    float textSpeed;

    TeleType teleType; //get from mainText

    //temp
    [SerializeField]
    string[] allParagraphs; //needs to be something more robust that holds "DialoguePart"s which can be simple text or text + player dialogue choices
    int paragraphTotal;
    int paragraphCounter;
    string _currentParagraph; //maybe 

    System.Action OnDialogueEnd;
    //System.Action OnLineEnd;

    //
    public void SetText(string[] incomingParagraphs)
    {
        allParagraphs = incomingParagraphs;
        //start read loop
    }

    private void Start()
    {
        teleType = mainText.gameObject.GetComponent<TeleType>();
        if(allParagraphs!=null && allParagraphs.Length >0)
        {
            StartCoroutine(ReadLoop());
        }
    }

    IEnumerator ReadLoop()
    {
        if(allParagraphs == null || allParagraphs.Length == 0)
        {
            print("no paragraphs to show");
            yield break;
        }

        paragraphTotal = allParagraphs.Length;
        paragraphCounter = 0;

        while (paragraphCounter < paragraphTotal)
        {
            //print paragraph

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return new WaitForEndOfFrame();
            StartCoroutine(ListenForSkip());
            _currentParagraph = allParagraphs[paragraphCounter];
            //teleType.SetAndPlayOnce(_currentParagraph);
            teleType.SetAndPlayOnce(_currentParagraph);
            paragraphCounter++;
            yield return new WaitUntil(()=> teleType.isReady);

            //yield return new WaitForEndOfFrame();

            StopCoroutine(ListenForSkip());
        }

        //On read-loop ended
        OnDialogueEnd?.Invoke();
    }

    IEnumerator ListenForSkip()
    {
        yield return new WaitUntil(()=>Input.anyKeyDown);
        teleType.Skip();
    }
    //public bool TryNext()
    //{

    //}
}
