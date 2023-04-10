using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curse : MonoBehaviour
{
    Color warningColor = new(0.8f, 0.1f, 0.1f);
    Color endColor = new(0f,0.45f,1f);
    Image curse;
    void Awake()
    {
        curse = GetComponent<Image>();
    }
    public void CurseAlarm()
    {
        curse.DOColor(warningColor, 4.7f).SetEase(Ease.Flash,36,-1);
    }
    
    public void EndAlarm()
    {
        curse.DOColor(endColor, 4.7f).SetEase(Ease.Flash, 36, -1);
    }
}
