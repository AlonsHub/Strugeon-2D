using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    public string characterName; //will this be enough to ref the merc-prefab

    int _experience = 0;
    int _level = 1;

    public int _expToNextLevel => GameStats.ExpThresholdByLevel(_level);
    public int _minDamageBonus => GameStats.minDmgPerLevel * _level;
    public int _maxDamageBonus => GameStats.maxDmgPerLevel * _level;
    public int _maxHpBonus => GameStats.maxHpBonusPerLevel * _level;

    public void ResetSheet()
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
            //levelup!
            _level++;
            return true;
        }
        return false;
    }

    
}
