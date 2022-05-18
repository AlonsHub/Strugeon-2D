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

    //being phased in, only relevant for BasicDisplayer SetMe overload
    [SerializeField]
    BasicDisplayer basicDisplayer;

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

        root.transform.position =  (Input.mousePosition.x > Screen.width/2) ? Input.mousePosition+ offset/1.5f : Input.mousePosition - offset / 1.5f;

        //scale me?
    }
    public void SetMe(string newText, Transform trans)
    {
        root.SetActive(true);

        textBox.text = newText;
        //position me
        root.transform.position = (Input.mousePosition.x > Screen.width / 2) ? trans.position + offset : trans.position - offset;

        //scale me?
    }
    public void SetMe(List<string> strings, List<Sprite> sprites)
    {
        root.SetActive(true);

        basicDisplayer.SetMe(strings, sprites);

        //textBox.gameObject.SetActive(false);
        //position me
        root.transform.position = (Input.mousePosition.x > Screen.width / 2) ? Input.mousePosition + offset : Input.mousePosition - offset;


        //scale me?
    }

    public void UnSetMe()
    {
        root.SetActive(false);
        if(textBox)
        textBox.text = ""; //not really needed...
    }

}
