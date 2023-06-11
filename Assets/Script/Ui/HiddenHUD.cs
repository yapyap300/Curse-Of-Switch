using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenHUD : MonoBehaviour
{

    Slider mySlider;

    void Awake()
    {
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        float curHealth = HiddenManager.Instance.boss.health;
        float maxHealth = HiddenManager.Instance.boss.maxHelth;
        mySlider.value = curHealth / maxHealth;
    }
}
