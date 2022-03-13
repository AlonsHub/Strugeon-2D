using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningAmmoItem : ActionItem, SA_Item
{
    WeaponItem weaponItem;
    [SerializeField]
    int percentChance;
    [SerializeField]
    Sprite burnningAmmoSprite;

    [SerializeField]
    GameObject burningGroundPrefab; //TBF - improve approach

    [SerializeField]
    int minDamage, maxDamage;

    public bool SA_Available()
    {
        return (weaponItem && weaponItem.hasEffect);
    }

    public string SA_Description()
    {
        return "When Tikvas missiles are LIT with fire, they burst on impact - burning all tiles around the target for 2 turns";
    }

    public string SA_Name()
    {
        return "Burning Ground";
    }

    public Sprite SA_Sprite()
    {
        return burnningAmmoSprite;
    }

    public void StartCooldown()
    {
        return; //cooldown is not used in this case
    }

    // Start is called before the first frame update
    private void Start()
    {
        weaponItem = GetComponent<WeaponItem>();
        weaponItem.attackAction += OnAttack;
    }

    void OnAttack()
    {
        if(SA_Available())
        {
            //roll on chance!
            int roll = Random.Range(0, 100);
            if(roll <= percentChance)
            weaponItem.cachedProjectile.AddComponent<BurnAmmo>().SetMe(minDamage, maxDamage, burningGroundPrefab);
        }
    }

    public override void CalculateVariations()
    {
        return;
    }
}
