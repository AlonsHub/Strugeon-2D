using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHandsItem : ActionItem, SA_Item
{
    //private Character ownerCharacter;
    public int minDamage;
    public int maxDamage;

    public int minDamagePerLevel;
    public int maxDamagePerLevel;



    public GameObject weaponGfx;
    public Transform weaponSpawn;

    public Pawn toHit;

    LookAtter la;
    /// <summary>
    /// Big hands can pick up things from around the level and throw them as weapons
    /// These can be Rocks and Censer(?) atm
    /// 
    /// </summary>
    /// 

    [SerializeField]
    Sprite saSprite;

    void Start()
    {
        //ownerCharacter = GetComponent<Character>();
        actionVariations = new List<ActionVariation>();
        la = GetComponentInChildren<LookAtter>();
    }

    
    public override void Action(GameObject tgt)
    {
        //return if no hopeful targets exist for some reason
        if (RefMaster.Instance.mercs.Count == 0)
            return;


        //tgt is the tile on which the obstacle is sitting
        FloorTile ft = tgt.GetComponent<FloorTile>();

        //Destroy(ft.myOccupant);
        ft.RemoveOccupant(true);
        
        toHit = RefMaster.Instance.mercs[Random.Range(0, RefMaster.Instance.mercs.Count - 1)]; //should be simplified
        //LookAtter la = GetComponentInChildren<LookAtter>();
        if (toHit && la)
            la.tgt = toHit.transform;
        
        pawn.anim.SetTrigger("Throw"); // animation calls event ShootRock();
    }
    public void ShootRock()//Called as animationevent by Throw animation
    {

        GameObject go = Instantiate(weaponGfx, weaponSpawn.position, Quaternion.identity);
        
        Arrow arr = go.GetComponent<Arrow>();
        if (!toHit)
        {
            Debug.LogError("No to hit for some fucking reason for: " + name);
        }
        
        arr.SetTarget(toHit.transform);

        
        if (la && toHit)
            la.tgt = toHit.transform;

        StartCoroutine(WaitForArrowToHit(go));
    } 
   
    IEnumerator WaitForArrowToHit(GameObject arrow) //or die, currently always hits
    {
        yield return new WaitUntil(() => (arrow == null));
        Debug.Log("Boulder hit");
        DamageTargetRock();
        yield return new WaitForSeconds(.2f);

        la.tgt = null;

        //pawn.TurnDone = true;
        pawn.FinishAnimation();
    }
    public void DamageTargetRock()
    {
        int rolledDamage = Random.Range(minDamage, maxDamage+1);
        toHit.TakeDamage(rolledDamage); // add time delay to reduce HP only after hit (atm this is done in TakeDamage and ReduceHP methods in character)
        BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Rock, toHit.Name, rolledDamage, Color.red);
    }
    

    public override void CalculateVariations()
    {
        
        actionVariations.Clear();
        List<FloorTile> neighbours = FloorGrid.Instance.GetNeighbours(pawn.tileWalker.currentNode);
        foreach(FloorTile ft in neighbours)
        {
            int weight = baseweight; //4

            //if(!ft.isEmpty && ft.myOccupant.GetComponent<ObstacleRock>()) //if it is an obstacle rock, we dont really need to cache it -
            //just to check that it is... maybe test by tag instead?
            if (!ft.isEmpty && ft.myOccupant) //linq Where would make this really easy TBF
            {
                if (ft.myOccupant.CompareTag("Rock"))
                {
                    if(pawn.isEnemy)//should just be aimed at "Targets"
                    {
                        int counter = 0;
                        foreach (var m in RefMaster.Instance.mercs)
                        {
                            //if (pawn.tileWalker.currentNode.GetDistanceToTarget(m.tileWalker.currentNode)/14 >= 2)
                            if (pawn.tileWalker.GetDistanceFromMeToYou(m.tileWalker)/14 >=2)
                            {
                                counter++;
                            }
                        } 
                        if(counter>=2)
                        {
                            weight *= 5;
                        }
                    }

                    actionVariations.Add(new ActionVariation(this, ft.gameObject, weight)); //no other calculations involved yet
                }
            }
        }
    }
    public bool SA_Available()
    {
        List<FloorTile> neighbours = FloorGrid.Instance.GetNeighbours(pawn.tileWalker.currentNode);
        foreach (FloorTile ft in neighbours)
        {
            //if(!ft.isEmpty && ft.myOccupant.GetComponent<ObstacleRock>()) //if it is an obstacle rock, we dont really need to cache it -
            //just to check that it is... maybe test by tag instead?
            if (!ft.isEmpty && ft.myOccupant)
            {
                if (ft.myOccupant.CompareTag("Rock"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int CurrentCooldown()
    {
        return -1;
    }

    public void StartCooldown()
    {
        return;
    }

    public Sprite SA_Sprite()
    {
        return saSprite;
    }

    public string SA_Name()
    {
        return "BigHandsItem";
    }

    public string SA_Description()
    {
        throw new System.NotImplementedException();
    }

    public void SetToLevel(int level)
    {
        minDamage += minDamagePerLevel * (level - 1);
        maxDamage += maxDamagePerLevel * (level - 1);
    }
}
