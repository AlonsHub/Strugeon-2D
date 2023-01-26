using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceTESTER : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Prices.GPL());
        Debug.Log(Prices.ApproximateGPL());
        for (int i = 0; i < 11; i++)
        {
            Debug.Log(Prices.ApproximateGPL(i));
        }
    }

   
}
