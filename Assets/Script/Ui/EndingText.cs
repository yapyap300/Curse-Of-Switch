using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingText : MonoBehaviour
{
    Text mytext;
    void Awake()
    {
        mytext= GetComponent<Text>();
    }
    void OnEnable()
    {
        Sequence mysequence = DOTween.Sequence();
        mysequence.Append(mytext.DOText("<color=red><b>£����</b></color> ���ִ� ������ Ǯ���� �ʴ´�...", 5f, true, ScrambleMode.Custom, "���ְ��ɸ�����������"))
            .Append(mytext.DOFade(0f, 3f));
        mysequence.Play();
    }
}
