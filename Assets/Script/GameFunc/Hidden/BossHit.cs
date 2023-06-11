using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Weapon")) return;

        HiddenManager.Instance.boss.health -= collision.GetComponent<Weapon>().damage / 2f;
        if (HiddenManager.Instance.boss.health < 0)
            HiddenManager.Instance.GameEnd();
    }
}
