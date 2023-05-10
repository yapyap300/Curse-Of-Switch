using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            Player myplay = collision.GetComponent<Player1>();
            myplay.Heal();
            SoundManager.Instance.PlaySfx("Heal");
            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Player2"))
        {
            Player myplay = collision.GetComponent<Player2>();
            myplay.Heal();
            SoundManager.Instance.PlaySfx("Heal");
            gameObject.SetActive(false);
        }
    }
}
