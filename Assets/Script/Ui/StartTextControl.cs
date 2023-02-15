using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class StartTextControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] TextMeshProUGUI Next;
    // Start is called before the first frame update
    void Start()
    {
        Title.DOFade(1,8.45f);
        Next.DOFade(0.45f,2f).SetLoops(-1, LoopType.Yoyo);
    }

}
