using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyStrikeComponent : MonoBehaviour //NOT AN ACTION ITEM!!!
{
    [SerializeField]
    WeaponItem weaponItem;

    [SerializeField]
    int maxDmg;
    [SerializeField]
    int minDmg;

    [Tooltip("As % chance to occur")]
    [SerializeField]
    int chanceToProc; //out of a hundred

    [SerializeField]
    GameObject addonPrefab;
    private void Awake()
    {
        weaponItem = GetComponent<WeaponItem>();
    }
    private void OnEnable()
    {
        weaponItem.attackAction += RollToProcOnAttack;
    }
    private void OnDisable()
    {
        weaponItem.attackAction -= RollToProcOnAttack;
    }
    public void RollToProcOnAttack()
    {
        //roll chance (15% as GDD)
        int roll = Random.Range(1, 101);

            Debug.Log("Something!");
        if(roll < chanceToProc)
        {
            Debug.Log("Yes strike");
            //add extra VFX
            //add extra damage (perhaps with its own dmg txt)
            weaponItem.ExtraDamageTarget(minDmg, maxDmg);

            List<FloorTile> neigbourTiles = FloorGrid.Instance.GetNeighbours(FloorGrid.Instance.GetTileByIndex(weaponItem.pawn.tileWalker.gridPos));
            foreach (var neighbour in neigbourTiles) //linq Where would simplify this TBF
            {
                //if(neighbour.isEmpty || !neighbour.myOccupant) //should never have happened here
                //{
                //    neighbour.isEmpty = true;
                //    neighbour.myOccupant = null; //makes sure that IF ONE then ALSO THE OTHER 
                //    continue;
                //}
                Pawn check = neighbour.myOccupant.GetComponent<Pawn>();
                if (check == null)
                {
                    continue;
                }
                if(check.isEnemy)
                {
                    check.TakeDamage(Random.Range(minDmg, maxDmg));
                }
            }

            BattleLogVerticalGroup.Instance.AddToNextEntry(addonPrefab);
        }
        else
        {
            Debug.Log("No strike");
        }
    }
}
