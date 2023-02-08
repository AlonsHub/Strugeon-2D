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
    [SerializeField]
    Transform dump;
    [SerializeField, Tooltip("")]
    GameObject gfx;



    List<VariationSimpleDisplayer> enabledDisplayers;
    List<VariationSimpleDisplayer> disbledDisplayers;

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
        enabledDisplayers = new List<VariationSimpleDisplayer>();
        disbledDisplayers = new List<VariationSimpleDisplayer>();
    }

    public void ToggleGFX()
    {
        gfx.SetActive(!gfx.activeSelf);
    }

    public void Set(Pawn p, int index)
    {
        pawn = p;
        ShowLog(index);
    }

    public void ShowLog(int index)
    {
        if (!pawn || !gfx.activeSelf)
            return;
        
        if(pawn.actionPool.Count > enabledDisplayers.Count)
        {
            int d = pawn.actionPool.Count - enabledDisplayers.Count;
            for (int i = 0; i < d; i++)
            {
                AddDisplayer();
            }
        }
        else if( enabledDisplayers.Count > pawn.actionPool.Count)
        {
            int d = enabledDisplayers.Count - pawn.actionPool.Count;

            for (int i = 0; i < d; i++)
            {
                DisableDisplayer();
            }
        }

        for (int i = 0; i < pawn.actionPool.Count; i++)
        {
            enabledDisplayers[i].SetMe(pawn.actionPool[i], (index == i)? Color.red : Color.white);
            //if(i==index)
            //{
            //    enabledDisplayers[i].GetComponentInParent<UnityEngine.UI.Image>().color = Color.red;
            //}
        }
        chosenActionText.text = $"{pawn.actionPool[index].relevantItem} on {pawn.actionPool[index].target.name}: {pawn.actionPool[index].weight}";
    }

    void AddDisplayer()
    {
        if (disbledDisplayers.Count == 0)
        {
            //TMP_Text toReturn = Instantiate(prefab, grid).GetComponentInChildren<TMP_Text>();
            VariationSimpleDisplayer toReturn = Instantiate(prefab, grid).GetComponent<VariationSimpleDisplayer>();
            //InitDisplayer(toReturn);
            enabledDisplayers.Add(toReturn);
        }
        else
        {
            EnableDisplayer();
        }
       
    }
    void DisableDisplayer()
    {
        VariationSimpleDisplayer t = enabledDisplayers[enabledDisplayers.Count - 1];
        disbledDisplayers.Add(t);
        enabledDisplayers.Remove(t);

        t.transform.SetParent(dump);
        t.transform.gameObject.SetActive(false);
    }
    void EnableDisplayer()
    {
        VariationSimpleDisplayer t = disbledDisplayers[0];
        //InitDisplayer(t);
        enabledDisplayers.Add(t);
        disbledDisplayers.Remove(t);

        t.transform.SetParent(grid);
        t.transform.gameObject.SetActive(true);
    }
}
