using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchBar : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;

    [SerializeField]
    GameObject searchBarable;

    ISearchBarable _searchBarable;

    private void Awake()
    {
        _searchBarable = searchBarable.GetComponent<ISearchBarable>();
    }

    public void OnValueChanged()
    {
       if(string.IsNullOrEmpty(inputField.text))
        {
            _searchBarable.ClearSearch();
        }
       else
        {
            _searchBarable.Search(inputField.text);
        }

    }
}
