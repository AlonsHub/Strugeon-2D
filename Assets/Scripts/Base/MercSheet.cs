using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MercAssignment {Null,AwaySquad,Room,Available, Hireable, NotAvailable};

[System.Serializable]
public class MercSheet
{
    public MercName characterName; //will this be enough to ref the merc-prefab
    public MercAssignment currentAssignment;
    public int roomOrSquadNumber =-1; //defualt -1 means merc doesn't belong to any squad, wheter in-room or away

    public int _experience = 0;
    public int _level = 1;

    public int _expToNextLevel => GameStats.ExpThresholdByLevel(_level);
    public Vector2Int _expFromAndToNextLevel => GameStats.ExpThresholdsByLevel(_level);
    public int _minDamageBonus => GameStats.minDmgPerLevel * (_level-1); //you only start gaining bonuses from level 2
    public int _maxDamageBonus => GameStats.maxDmgPerLevel * (_level - 1);
    public int _maxHpBonus => GameStats.maxHpBonusPerLevel * (_level - 1);

    public System.Action LevelUpAction;

    public MercSheet()
    {
        characterName = MercName.None;

        ResetSheet(); //exp and level set to base
        currentAssignment = MercAssignment.Null; //maybe don't do this, but keep it now so nothing accidently happens
        roomOrSquadNumber = -2; //still unclear 
    }
    public MercSheet(MercName mercName)
    {
        characterName = mercName;

        ResetSheet(); //exp and level set to base
        currentAssignment = MercAssignment.Null; //maybe don't do this, but keep it now so nothing accidently happens
        roomOrSquadNumber = -1; //still unclear 
    }
    public MercSheet(MercName mercName, MercAssignment assignment, int relevantNum) //-1 == number not relevant. Assignment decides if they're Available, Hireable or In a Room/AwaySquad
    {
        characterName = mercName;

        ResetSheet(); //exp and level set to base
        SetToState(assignment, relevantNum); 
    }

    //public GameObject MyPawnPrefabRef() //might still prefer this for some reason if EnumToT proves to be a brat
    //{
    //    return MercPrefabs.Instance.EnumToPrefab(characterName);
    //}
    public T MyPawnPrefabRef<T>()
    {
        if (MercPrefabs.Instance)
            return MercPrefabs.Instance.EnumToT<T>(characterName);
        else
            throw new System.Exception();
    }

    public void ResetSheet() //DOES NOT RESET ASSIGNMENT YET!
    {
        _experience = 0;
        _level = 1;
    }

    public bool AddExp(int exp) //return true if level-up, not sure if needed. Just if/when exp is added, it may need to wait for some animations and/or prompt another
    {
        _experience += exp;
        int start = GameStats.expToLevel2;
        int pre = 0;
        int threshhold = 0;

        for (int i = 1; i <= _level; i++)
        {
            threshhold = pre + start;
            pre = start;
            start = threshhold;
        }

        if (_experience >= threshhold)
        {
            LevelUp();
            return true;
        }
        return false;
    }
    //[ContextMenu("levelUp")]
    public void LevelUp()
    {
        _level++;
        //call OnLevelUp!
        IdleLogOrder newOrder = new IdleLogOrder(PrefabArchive.Instance.GetPrefabByDisplayerType(DisplayerType.LevelUp), new List<string> { characterName.ToString() + " advanced!", $"from {_level - 1} to {_level}"}, MyPawnPrefabRef<Pawn>().PortraitSprite);
        IdleLog.AddToBackLog(newOrder, true);
        LevelUpAction?.Invoke();
    }


    public void SetToState(MercAssignment assignment, int relevantNum)
    {
        //switch (assignment)
        //{
        //    case MercAssignment.Null:
        //        Debug.LogWarning("Merc assignment shouldn't be turned into null as far as I know, but for some reason - we're here now");
        //        //could be useful for resetting?
        //        break;
        //    case MercAssignment.AwaySquad:
        //        ro
        //        break;
        //    case MercAssignment.Room:
        //        break;
        //    case MercAssignment.Available:
        //        break;
        //    default:
        //        break;
        //}
        currentAssignment = assignment;
        roomOrSquadNumber = relevantNum;
    }
}
