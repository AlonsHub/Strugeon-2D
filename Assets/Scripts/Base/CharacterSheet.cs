using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    int _experience;
    int _level;
    int _expToNextLevel;

    public int _minDamageBonus => GameStats.minDmgPerLevel * _level;
    public int _maxDamageBonus => GameStats.maxDmgPerLevel * _level;

    public CharacterSheet()
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
