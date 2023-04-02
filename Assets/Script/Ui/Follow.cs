using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] Camera cam;
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if(id == 1)
            rect.position = cam.WorldToScreenPoint(GameManager.Instance.player1.transform.position);
        else
            rect.position = cam.WorldToScreenPoint(GameManager.Instance.player2.transform.position);
    }
}
