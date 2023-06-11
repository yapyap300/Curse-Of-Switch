using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            HiddenManager.Instance.player1.health -= 50;
            if(HiddenManager.Instance.player1.health < 0)
            {
                HiddenManager.Instance.player1.Dead();
            }
        }
        else if (collision.CompareTag("Player2"))
        {
            HiddenManager.Instance.player2.health -= 50;
            if (HiddenManager.Instance.player2.health < 0)
            {
                HiddenManager.Instance.player2.Dead();
            }
        }
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
