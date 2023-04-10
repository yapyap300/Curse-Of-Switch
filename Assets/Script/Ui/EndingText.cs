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
        mysequence.Append(mytext.DOText("<color=red><b>짙어진</b></color> 저주는 영원히 풀리지 않는다...", 5f, true, ScrambleMode.Custom, "저주가걸릴때맞지마라"))
            .Append(mytext.DOFade(0f, 3f));
        mysequence.Play();
    }
}
