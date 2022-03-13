using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurnAmmo : MonoBehaviour
{
    FloorTile ft;
    List<FloorTile> targets;

    GameObject tileFireEffect;

    int min, max;
    private void Start()
    {

        ft = FloorGrid.Instance.GetTileByPosition(GetComponent<Arrow>().tgt.position);
        targets = FloorGrid.Instance.GetNeighbours(ft);
        targets = targets.Where(x => x.isEmpty || (!x.isEmpty && (x.myOccupant.GetComponent<Pawn>()))).ToList(); //logic for tile that wont burn

    }

    public void SetMe(int minDamage, int maxDamage, GameObject tileEffectPrefab)
    {
        tileFireEffect = tileEffectPrefab;
        min = minDamage;
        max = maxDamage;
    }

    private void OnDestroy()
    {
        foreach (var item in targets)
        {
            BurningTileEffect tileEffect = Instantiate(tileFireEffect).GetComponent<BurningTileEffect>();

            tileEffect.SetEffect(item, min, max);
        }
    }


    //set damage

}
