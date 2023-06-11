using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenOver : MonoBehaviour
{
    Image over;
    void Awake()
    {
        over = GetComponent<Image>();
    }
    void OnEnable()
    {
        over.transform.DOPunchScale(Vector3.one * 10f, 5f, 10, 0f).SetUpdate(true).OnComplete(() => Application.Quit());
    }
}
