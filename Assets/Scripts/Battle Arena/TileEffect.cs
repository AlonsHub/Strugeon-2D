using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    // Attached to FloorTiles - usually sets an effect + sets the floor-tiles TileElement
    [SerializeField]
    TileElement effectElement; //none if irrelavent
    TileElement? oldTileElement = null; //NULL if irrelavent

    [Tooltip("total duration of the effect, in turns")]
    [SerializeField]
    int effectDuration; //total duration in turns/uses/both? //GDD says 2 turns

    int _currentDuration;

    FloorTile floorTile;
    public void SetMe(FloorTile tile)
    {
        _currentDuration = 0;
        floorTile = tile;
        if (effectElement != TileElement.None) //currently None doesnt undo an existing effect on a tile
        {
            if (floorTile.tileElement != TileElement.None)
            {
                oldTileElement = floorTile.tileElement;
            }
            floorTile.tileElement = effectElement;
        }

        floorTile.OnOccupantEnter += ApplyEffectToOccupant;
        TurnMaster.Instance.OnTurnOrderRestart += ReduceDurationByOne;
    }
    public void SetMe(Vector2Int gridpos)
    {
        _currentDuration = 0;
        floorTile = FloorGrid.Instance.GetTileByIndex(gridpos);
        if (effectElement != TileElement.None) //currently None doesnt undo an existing effect on a tile
        {
            if(floorTile.tileElement != TileElement.None)
            {
                oldTileElement = floorTile.tileElement;
            }
            floorTile.tileElement = effectElement;
        }
        floorTile.OnOccupantEnter += ApplyEffectToOccupant;
        TurnMaster.Instance.OnTurnOrderRestart += ReduceDurationByOne;
    }
    private void OnDisable()
    {
        TurnMaster.Instance.OnTurnOrderRestart -= ReduceDurationByOne;
        floorTile.OnOccupantEnter -= ApplyEffectToOccupant;

    }

    public virtual void ApplyEffectToOccupant()
    {

    }

    void ReduceDurationByOne()
    {
        _currentDuration--;
        if(_currentDuration<=0)
        {
            EndEffect();
        }
    }

    public virtual void EndEffect()
    {
        //
        floorTile.tileElement = effectElement;


        Destroy(gameObject);
    }

    


}
