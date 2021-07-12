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

    public GameObject effectGFXPrefab;
}
