using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleLogOrder 
{
    public GameObject specificDisplayerPrefab; //with BasicDisplayer on it
    public List<string> strings;
    public List<Sprite> sprites;

    public bool isPersistent = false; //this is set in the static AddToBackLog method, since it's mostly false and doesnt warrent a spot in a constructor

    
    //Single Inputs
    public IdleLogOrder(GameObject prefab, string _string) 
    {
        specificDisplayerPrefab = prefab;
        strings = new List<string> { _string };
    }
    public IdleLogOrder(GameObject prefab, Sprite _sprite) 
    {
        specificDisplayerPrefab = prefab;
        sprites = new List<Sprite> { _sprite };
    }
    public IdleLogOrder(GameObject prefab, string _string, Sprite _sprite) 
    {
        specificDisplayerPrefab = prefab;
        strings = new List<string> { _string };
        sprites = new List<Sprite> { _sprite };
    }
    //List Inputs
    public IdleLogOrder(GameObject prefab, List<string> _strings)
    {
        specificDisplayerPrefab = prefab;
        strings = _strings;
    }
    public IdleLogOrder(GameObject prefab, List<Sprite> _sprites)
    {
        specificDisplayerPrefab = prefab;
        sprites = _sprites;
    }
    public IdleLogOrder(GameObject prefab, List<string> _strings, List<Sprite> _sprites)
    {
        specificDisplayerPrefab = prefab;
        strings = _strings;
        sprites = _sprites;
    }
    //Mixed List & Single Inputs
    public IdleLogOrder(GameObject prefab, string _string, List<Sprite> _sprites)
    {
        specificDisplayerPrefab = prefab;
        strings = new List<string>{ _string};
        sprites = _sprites;
    }
    public IdleLogOrder(GameObject prefab, List<string> _strings, Sprite _sprite)
    {
        specificDisplayerPrefab = prefab;
        strings = _strings;
        sprites = new List<Sprite>{ _sprite};
    }


}
