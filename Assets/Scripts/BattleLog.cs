using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleLog : MonoBehaviour
{
    public static BattleLog Instance;
    public Transform content;
    public GameObject textPrefab;

    //TEMP AF!
    public int numberOfLines;
    public float lineSize;
    int countOfLines;
    private void Awake()
    {
        countOfLines = 0;
        Instance = this;
    }

    public TMP_Text logText;
    public ScrollRect scrollRect;
    
    public void AddLine(string newLine)
    {
       Instantiate(textPrefab, content);
       textPrefab.GetComponent<TMP_Text>().text = newLine;
       scrollRect.normalizedPosition = new Vector2(0, 0);
    }
}
