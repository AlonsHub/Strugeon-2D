using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHoverDisplayer : MonoBehaviour
{
    public static CharacterHoverDisplayer Instance;

    [SerializeField]
    GameObject displayParent;

    [SerializeField]
    TMP_Text nameDisplayer;
    [SerializeField]
    TMP_Text currentHPDisplayer;
    [SerializeField]
    TMP_Text damageDisplayer;
    [SerializeField]
    TMP_Text extraAttributeDisplayer;
    [SerializeField]
    TMP_Text specialAttributeDisplayer;
    //[SerializeField]
    //Image specialIconDisplayer;

    private void Awake()
    {
        if(Instance !=null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        displayParent.SetActive(false);
    }

    public void SetHoverToMerc(Pawn pawn, float xValue)
    {
        nameDisplayer.text = pawn.Name;
        currentHPDisplayer.text = pawn.currentHP.ToString();
        damageDisplayer.text = pawn.GetComponent<WeaponItem>().damage.ToString();
        transform.position = new Vector3(xValue, transform.position.y, transform.position.z);

         
        //attributes need to be set in pawn, make all special action items that are player-specific have the same interface/abastract-class
    }

    public void OnOffToggel(bool isOn)
    {
        displayParent.SetActive(isOn);
    }



}
