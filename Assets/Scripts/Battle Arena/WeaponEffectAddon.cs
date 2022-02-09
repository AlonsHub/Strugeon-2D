using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DamageElement {Fire, Electricity, Acid, Psychic}
public enum WeaponEffect { blazing, electric, frozen, crused, non };

[System.Serializable]
[CreateAssetMenu()]
public class WeaponEffectAddon : ScriptableObject
{
    public EffectAddonDataType data;
}
[System.Serializable]
public class EffectAddonDataType
{
    public int bonusDamage;
    public WeaponEffect damageElement;

    public int currentUses;
    public int maxUses;

    public GameObject effectGFXPrefab;

    public void SetMe(EffectAddonDataType otherData)
    {
        bonusDamage = otherData.bonusDamage;
        currentUses = maxUses = otherData.maxUses;
        damageElement = otherData.damageElement;
    }
}
