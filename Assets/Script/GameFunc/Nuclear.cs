using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuclear : MonoBehaviour
{
    Transform nuclear;
    void Awake()
    {
        nuclear = transform.GetChild(0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            nuclear.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
