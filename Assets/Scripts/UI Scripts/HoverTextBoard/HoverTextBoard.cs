using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverTextBoard : MonoBehaviour
{
    public static HoverTextBoard Instance;
    [SerializeField]
    GameObject root; //the root object for the displayer
    [SerializeField]
    TMP_Text textBox;
    [SerializeField]
    private Vector3 offset;

    void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        root.SetActive(false);
    }
    public void SetMe(string newText)
    {
        root.SetActive(true);

        textBox.text = newText;
        //position me
        root.transform.position = Input.mousePosition+ offset;

        //scale me?
    }
    public void SetMe(string newText, Transform trans)
    {
        root.SetActive(true);

        textBox.text = newText;
        //position me
        root.transform.position = trans.position + offset;

        //scale me?
    }
    public void UnSetMe()
    {
        root.SetActive(false);

        textBox.text = "";
    }

}
