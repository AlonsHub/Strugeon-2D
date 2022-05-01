using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealthPrefs
{
    public HealthPercentPref[] percentModParis;
}
//public enum ComparisonType { GreaterThan, LessThan};
[System.Serializable]
public class HealthPercentPref
{
    public int percentOfHealth;
    public int modifier;

    //public ComparisonType comparisonType; //easily applicable

}
