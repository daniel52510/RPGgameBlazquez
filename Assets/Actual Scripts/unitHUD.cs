using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class unitHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider hpSlider;
    public Slider mpSlider;
    public Text nameGO;
    public void setHUD(Unit unit)
    {
        nameGO.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.currentMP;
    }
    public void updateHPMP(int hp, int mp)
    {
        hpSlider.value = hp;
        mpSlider.value = mp;
    }
}
