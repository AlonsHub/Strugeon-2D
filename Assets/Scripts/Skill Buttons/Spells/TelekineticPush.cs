using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekineticPush : SpellButton
{
    [SerializeField]
    Vector2Int direction;
    [SerializeField]
    int tilesToMove = 2;
    [SerializeField]
    private int damage;

    public override void OnButtonClick()
    {
        
        pawnTgt = MouseBehaviour.hitTarget;

        //Enable directional pad

        StartCoroutine(nameof(Push));

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Blue, SturgeonColours.Instance.noolBlue);


        base.OnButtonClick();
    }

    //!!! Override interactableCheck to make sure that the "illegal" directional buttons will gray out when in map corners

    IEnumerator Push()
    {
        //Check if can push in direction at-all
        for (int i = 0; i < tilesToMove; i++)
        {
            if (!pawnTgt.tileWalker.StepInDirection(direction))
            {
                //damage occupant
                FloorGrid.Instance.floorTiles[pawnTgt.tileWalker.currentNode.gridIndex.x + direction.x, pawnTgt.tileWalker.currentNode.gridIndex.y + direction.y].myOccupant.GetComponent<LiveBody>().TakeDamage(damage);

                break;
            }
            yield return new WaitForSeconds(0.2f);
            //Check if direction will hit obstacle or turntaker
        }

    }
}
