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

    [SerializeField]
    GameObject endParagraphPointer;//drag this! //can be replaced by adding a <sprite=2> to the end of each paragraph
    //
    public void SetText(string[] incomingParagraphs)
    {
        allParagraphs = incomingParagraphs;
        //start read loop
    }

    private void Start()
    {
        teleType = mainText.gameObject.GetComponent<TeleType>();
        if (allParagraphs != null && allParagraphs.Length > 0)
        {
            StartCoroutine(ReadLoop());
        }
    }

    IEnumerator ReadLoop()
    {
        if (allParagraphs == null || allParagraphs.Length == 0)
        {
            print("no paragraphs to show");
            yield break;
        }

        paragraphTotal = allParagraphs.Length;
        paragraphCounter = 0;
        endParagraphPointer.SetActive(false);

        //check if this Dialogue is "Instant start" or "click to start" TBF
        if (true)//for now assume yes //if this turns false, paragraphCounter stays the same - nothing needs to change in following logic
        {
            //_currentParagraph = allParagraphs[paragraphCounter];
            //paragraphCounter++;
            StartCoroutine(ListenForSkip());
            _currentParagraph = allParagraphs[paragraphCounter]; //0 in this case

            teleType.SetAndPlayOnce(_currentParagraph);
            paragraphCounter++; // grows to 1
            yield return new WaitUntil(() => teleType.isReady);
            StopCoroutine(ListenForSkip());
            endParagraphPointer.SetActive(true);
            //yield return new WaitForEndOfFrame();

        }

        while (paragraphCounter < paragraphTotal)
        {
            //print paragraph

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => Input.anyKeyDown);
            //yield return new WaitForEndOfFrame(); //this is enough, but I want more!
            yield return new WaitForSeconds(0.2f);
            endParagraphPointer.SetActive(false);
            StartCoroutine(ListenForSkip());
            _currentParagraph = allParagraphs[paragraphCounter];
            //teleType.SetAndPlayOnce(_currentParagraph);
            teleType.SetAndPlayOnce(_currentParagraph);
            paragraphCounter++;
            yield return new WaitUntil(() => teleType.isReady);
            StopCoroutine(ListenForSkip());
            endParagraphPointer.SetActive(true);
            //yield return new WaitForEndOfFrame();

        }

        //On read-loop ended
        OnDialogueEnd?.Invoke();
    }

    IEnumerator ListenForSkip()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        teleType.Skip();
    }

}
