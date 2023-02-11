 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int primary;
    public int secondary;
    public int special;
    public int numPotions;

    public bool takeDamage(int Damage)
    {
        currentHP -= Damage;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }
    public bool useMana(int Mana)
    {
        currentMP -= Mana;

        if (currentMP <= 0)
            return true;
        else
            return false;
    }

    public void heal(int num)
    {
        currentHP += num;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

}
