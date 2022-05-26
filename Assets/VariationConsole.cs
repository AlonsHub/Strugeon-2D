using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VariationConsole : MonoBehaviour
{
    public static VariationConsole Instance;

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform grid;
    [SerializeField, Tooltip("")]
    GameObject gfx;



    List<TMP_Text> enabledDisplayers;
    List<TMP_Text> disbledDisplayers;

    [SerializeField]
    TMP_Text chosenActionText;

    Pawn pawn;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        enabledDisplayers = new List<TMP_Text>();
        disbledDisplayers = new List<TMP_Text>();
    }

    public void Set(Pawn p, int index)
    {
        pawn = p;
        ShowLog(index);
    }

    public void ShowLog(int index)
    {
        if (!pawn)
            return;
        
        if(pawn.actionPool.Count > enabledDisplayers.Count)
        {
            for (int i = 0; i < pawn.actionPool.Count - enabledDisplayers.Count; i++)
            {
                AddDisplayer();
            }
        }
        else if( enabledDisplayers.Count > pawn.actionPool.Count)
        {
            for (int i = 0; i < enabledDisplayers.Count - pawn.actionPool.Count; i++)
            {
                DisableDisplayer();
            }
        }


        for (int i = 0; i < enabledDisplayers.Count; i++)
        {
            enabledDisplayers[i].text = $"{pawn.actionPool[i].relevantItem} on {pawn.actionPool[i].target.name}: {pawn.actionPool[i].weight}";
            if(i==index)
            {
                enabledDisplayers[i].GetComponentInParent<UnityEngine.UI.Image>().color = Color.red;
            }
        }
        chosenActionText.text = $"{pawn.actionPool[index].relevantItem} on {pawn.actionPool[index].target.name}: {pawn.actionPool[index].weight}";
    }

    TMP_Text AddSetDisplayer(string newText)
    {
        TMP_Text toReturn = Instantiate(prefab, grid).GetComponentInChildren<TMP_Text>();
        toReturn.text = newText;
        enabledDisplayers.Add(toReturn);
        return toReturn;
    }
    void AddDisplayer()
    {
        if (disbledDisplayers.Count == 0)
        {
            TMP_Text toReturn = Instantiate(prefab, grid).GetComponentInChildren<TMP_Text>();
            enabledDisplayers.Add(toReturn);
        }
        else
        {
            EnableDisplayer();
        }
       
    }
    void DisableDisplayer()
    {
        TMP_Text t = enabledDisplayers[enabledDisplayers.Count - 1];
        disbledDisplayers.Add(t);
        enabledDisplayers.Remove(t);
        t.transform.parent.gameObject.SetActive(false);
    }
    void EnableDisplayer()
    {
        TMP_Text t = disbledDisplayers[0];
        enabledDisplayers.Add(t);
        disbledDisplayers.Remove(t);
        t.transform.parent.gameObject.SetActive(false);
    }
}
