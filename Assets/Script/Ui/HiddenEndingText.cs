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
        mysequence.Append(mytext.DOText("���ְ� ����� ����", 5f))
            .Append(mytext.DOFade(0f, 3f)).Append(mytext.DOText("������ �ٿ��� �ٰ��´�!",3f));
        mysequence.Play();
    }
}
