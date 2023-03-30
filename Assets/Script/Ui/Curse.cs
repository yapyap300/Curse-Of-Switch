using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curse : MonoBehaviour
{
    Color myColor = new(0.8f, 0.1f, 0.1f);
    [SerializeField] Image curse;
    public void CurseRutine()
    {
        curse.DOColor(myColor,4.7f).SetEase(Ease.Flash,36,-1);
    }    
}
