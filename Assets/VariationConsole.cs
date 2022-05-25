using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VariationConsole : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform grid;
    [SerializeField, Tooltip("")]
    GameObject gfx;



    List<TMP_Text> enabledDisplayers;
    List<TMP_Text> disbledDisplayers;


    Pawn pawn;
    public void SetPawn(Pawn p)
    {
        pawn = p;
        ShowLog();
    }

    public void ShowLog()
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
        }
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
