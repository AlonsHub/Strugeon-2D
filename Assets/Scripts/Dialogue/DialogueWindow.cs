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

    [SerializeField]
    DialogueSequenceSO dialogueSequenceSO;

    //temp
    //string[] allParagraphs => dialogueSequenceSO.dialogueUnit; //needs to be something more robust that holds "DialoguePart"s which can be simple text or text + player dialogue choices
    int paragraphTotal;
    int paragraphCounter;
    string _currentParagraph; //maybe 
    DialogueUnit _currentUnit; //maybe 

    System.Action OnDialogueEnd;
    //System.Action OnLineEnd;

    [SerializeField]
    GameObject buttonTextPrefab;
    [SerializeField]
    Transform choiceGroup;
    [SerializeField]
    GameObject endParagraphPointer;//drag this! //can be replaced by adding a <sprite=2> to the end of each paragraph
    //
    public void SetText(DialogueSequenceSO newSO)
    {
        dialogueSequenceSO = newSO;
        //allParagraphs = incomingParagraphs;

        //start read loop
    }

    private void Start()
    {
        teleType = mainText.gameObject.GetComponent<TeleType>();
        if (dialogueSequenceSO != null && dialogueSequenceSO.dialogueUnits.Length > 0)
        {
            StartCoroutine(ReadLoop());
        }
    }

    IEnumerator ReadLoop()
    {
        if (dialogueSequenceSO == null || dialogueSequenceSO.dialogueUnits.Length == 0)
        {
            print("no paragraphs to show");
            yield break;
        }

        paragraphTotal = dialogueSequenceSO.dialogueUnits.Length;
        paragraphCounter = 0;
        endParagraphPointer.SetActive(false);

        //check if this Dialogue is "Instant start" or "click to start" TBF
        if (true)//for now assume yes //if this turns false, paragraphCounter stays the same - nothing needs to change in following logic
        {
            //_currentParagraph = allParagraphs[paragraphCounter];
            //paragraphCounter++;
            StartCoroutine(ListenForSkip());
            _currentUnit = dialogueSequenceSO.dialogueUnits[paragraphCounter];
            _currentParagraph = dialogueSequenceSO.dialogueUnits[paragraphCounter]._text; //0 in this case
            switch (_currentUnit.dialogueType)
            {
                case DialogueType.Spoken:
                    break;
                case DialogueType.Descriptive:
                    SetStyle('i');
                    break;
                case DialogueType.Choice:
                    SetStyle('b');
                    break;
                default:
                    break;
            }
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

            if(choiceGroup.childCount>0)
            {
                for (int i = choiceGroup.childCount-1; i >= 0; i--)
                {
                    Destroy(choiceGroup.GetChild(i).gameObject);
                }
            }

            StartCoroutine(ListenForSkip());
            
            _currentUnit = dialogueSequenceSO.dialogueUnits[paragraphCounter];


            _currentParagraph = dialogueSequenceSO.dialogueUnits[paragraphCounter]._text; //0 in this case
            switch (_currentUnit.dialogueType)
            {
                case DialogueType.Spoken:
                    break;
                case DialogueType.Descriptive:
                    SetStyle('i');
                    break;
                case DialogueType.Choice:
                    SetStyle('b');

                    if(_currentUnit.dialogueChoices !=null && _currentUnit.dialogueChoices.Count > 0)
                    {
                        foreach (var item in _currentUnit.dialogueChoices)
                        {
                            GameObject go = Instantiate(buttonTextPrefab, choiceGroup);
                            ChoiceTextButton choiceButton = go.GetComponent<ChoiceTextButton>();

                            choiceButton.SetMe(item);
                            choiceButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => RecieveChoice(item));
                        }
                    }    

                    break;
                default:
                    break;
            }

            teleType.SetAndPlayOnce(_currentParagraph);
            paragraphCounter++;
            yield return new WaitUntil(() => teleType.isReady);
            StopCoroutine(ListenForSkip());
            endParagraphPointer.SetActive(true);
        }

        //On read-loop ended
        OnDialogueEnd?.Invoke();
    }

    IEnumerator ListenForSkip()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        teleType.Skip();
    }

    void SetStyle(char c)
    {
        _currentParagraph= $"<{c}>{_currentParagraph}</{c}>";

    }

    public void RecieveChoice(DialogueChoice dc)
    {
        print(dc.flags);


    }

}
