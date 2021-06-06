using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHandsItem : ActionItem
{
    //private Character ownerCharacter;
    public int damage;
    public GameObject weaponGfx;
    public Transform weaponSpawn;

    public Pawn toHit;
    /// <summary>
    /// Big hands can pick up things from around the level and throw them as weapons
    /// These can be Rocks and Censer(?) atm
    /// 
    /// </summary>

    void Start()
    {
        //ownerCharacter = GetComponent<Character>();
        actionVariations = new List<ActionVariation>();
    }

    
    public override void Action(GameObject tgt)
    {
        //tgt is the tile on which the obstacle is sitting
        FloorTile ft = tgt.GetComponent<FloorTile>();
        ft.isEmpty = true;
        Destroy(ft.myOccupant);
        ft.myOccupant = null;

        

        //tgt.transform.GetChild(1).gameObject.SetActive(false);// destroy?

        //roll target
        //if(pawn.targets != null)
        //{
        //    pawn.targets = RefMaster.Instance.mercs;
        //    //pawn.targets.
        //}

        //toHit = pawn.targets[Random.Range(0, pawn.targets.Count - 1)]; //should be simplified
        toHit = RefMaster.Instance.mercs[Random.Range(0, RefMaster.Instance.mercs.Count - 1)]; //should be simplified
        LookAtter la = GetComponentInChildren<LookAtter>();
        if (toHit && la)
            la.tgt = toHit.transform;
        //toHit.TakeDamage(damage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        //GameObject go = Instantiate(weaponGfx, weaponSpawn.position, Quaternion.identity);
        //go.transform.LookAt(toHit.transform.position + Vector3.up*2f);
        //go.GetComponent<Arrow>().tgt = toHit.transform;

        //pawn.transform.LookAt(toHit.transform.position);




        pawn.anim.SetTrigger("Throw");
        //pawn.TurnDone = true;
        //StartCoroutine("CharacterThrow");
    }
    public override void CalculateVariations()
    {
        actionVariations = new List<ActionVariation>();
        List<FloorTile> neighbours = FloorGrid.Instance.GetNeighbours(pawn.tileWalker.currentNode);
        foreach(FloorTile ft in neighbours)
        {
            //if(!ft.isEmpty && ft.myOccupant.GetComponent<ObstacleRock>()) //if it is an obstacle rock, we dont really need to cache it -
            //just to check that it is... maybe test by tag instead?
            if (!ft.isEmpty && ft.myOccupant)
            {
                if (ft.myOccupant.CompareTag("Rock"))
                {
                    actionVariations.Add(new ActionVariation(this, ft.gameObject, baseCost)); //no other calculations involved yet
                }
            }
        }

    }
    //IEnumerator CharacterThrow()
    //{
    //    yield return new WaitForSeconds(ownerCharacter.delayAfterAttack);
    //    //if (ownerCharacter.doDoubleTurn)
    //    //    TurnOrder.Instance.currentTurn--;
    //    TurnOrder.Instance.NextTurn();
    //}
    public void ShootRock()
    {

        GameObject go = Instantiate(weaponGfx, weaponSpawn.position, Quaternion.identity);
        //go.transform.LookAt(toHit.transform.position + Vector3.up * 2f);
        Arrow arr = go.GetComponent<Arrow>();
        if (!toHit)
        {
            Debug.LogError("No to hit for some fucking reason for: " + name);
        }
        arr.tgt = toHit.transform;

        LookAtter la = GetComponentInChildren<LookAtter>();
        if (la)
            la.tgt = toHit.transform;

        StartCoroutine(WaitForArrowToHit(go));
    }
    IEnumerator WaitForArrowToHit(GameObject arrow) //or die, currently always hits
    {
        //yield return new WaitForSeconds(.1f);
        //arrow.GetComponent<Arrow>().tgt = toHit.transform;
        yield return new WaitUntil(() => (arrow == null));
        Debug.Log("Boulder hit");
        DamageTargetRock();
        yield return new WaitForSeconds(.2f);
        EndTurn();
    }
    public void DamageTargetRock()
    {
        float rolledDamage = damage + Random.Range(-5, 6);
        toHit.TakeDamage((int)rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Rock, toHit.Name, (int)rolledDamage, Color.red);
    }
    void EndTurn() //seperated for invoke-sake
    {
        pawn.TurnDone = true;
    }
}
