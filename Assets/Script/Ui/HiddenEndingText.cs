using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenEndingText : MonoBehaviour
{
    Text mytext;
    void Awake()
    {
        mytext= GetComponent<Text>();
    }
    void OnEnable()
    {
        Sequence mysequence = DOTween.Sequence();
        mysequence.Append(mytext.DOText("저주가 희미해 진다", 5f))
            .Append(mytext.DOFade(0f, 3f)).Append(mytext.DOText("저주의 근원이 다가온다!",3f));
        mysequence.Play();
    }
}
