using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Censer : MonoBehaviour, GridPoser, PurpleTarget
{
    [SerializeField]
    private string censerName;
    public string Name { get => censerName; }

    public GameObject asPurpleTgtGameObject => gameObject;

    public FloorTile currentNode;
    //public WeaponEffect weaponEffect;
    public WeaponEffectAddon effectAddon;
    public GameObject effectPrefab;
    public Sprite effectIcon;
    public Color effectColour;

    public int dmgBonus;
    public int duration;

    public Vector2Int gridPos;


    public Vector2Int GetGridPos()
    {
        return gridPos;
    }

    public string GetName()
    {
        return name;
    }

    public void SetGridPos(Vector2Int newPos)
    {
        gridPos = newPos;
        currentNode = FloorGrid.Instance.GetTileByIndex(gridPos);
        currentNode.isEmpty = false;
        currentNode.myOccupant = gameObject;
    }
    private void Start()
    {
        RefMaster.Instance.censer = this;
        currentNode = FloorGrid.Instance.GetTileByIndex(gridPos);

        currentNode.isEmpty = false;
        currentNode.myOccupant = gameObject;
    }
}
