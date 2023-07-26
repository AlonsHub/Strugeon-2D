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

    [Tooltip("display only! don't touch!")]
    [SerializeField]
    int _remainingDuration;

    public FloorTile floorTile;
    public void SetBase(FloorTile tile)
    {
        _remainingDuration = effectDuration;
        floorTile = tile;

        transform.position = floorTile.transform.position;
        transform.position += Vector3.up * .3f;

        if (effectElement != TileElement.None) //currently None doesnt undo an existing effect on a tile
        {
            if (floorTile.tileElement != TileElement.None)
            {
                oldTileElement = floorTile.tileElement;
            }
            floorTile.tileElement = effectElement;
        }

        floorTile.OnOccupantEnter += TryApplyEffectToOccupant;
        TurnMachine.Instance.OnStartNewRound += ReduceDurationByOne;
    }
    public void SetBase(Vector2Int gridpos)
    {
        _remainingDuration = effectDuration;
        floorTile = FloorGrid.Instance.GetTileByIndex(gridpos);

        transform.position = floorTile.transform.position;
        transform.position += Vector3.up * .3f;

        if (effectElement != TileElement.None) //currently None doesnt undo an existing effect on a tile
        {
            if(floorTile.tileElement != TileElement.None)
            {
                oldTileElement = floorTile.tileElement;
            }
            floorTile.tileElement = effectElement;
        }
        floorTile.OnOccupantEnter += TryApplyEffectToOccupant;
        TurnMachine.Instance.OnStartNewRound += ReduceDurationByOne;
    }
    protected void OnDisable()
    {
        if(TurnMachine.Instance)
        TurnMachine.Instance.OnStartNewRound -= ReduceDurationByOne;
        floorTile.OnOccupantEnter -= TryApplyEffectToOccupant;

    }

    public virtual void TryApplyEffectToOccupant()
    {

    }

    protected void ReduceDurationByOne()
    {
        _remainingDuration--;
        if(_remainingDuration<=0)
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
