using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosition : MonoBehaviour
{
    Collider2D coll;
    public int id;

    void Awake()
    {
        coll= GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        Vector3 PlayerPos;
        if(id == 1)
            PlayerPos = GameManager.Instance.player1.transform.position;
        else
            PlayerPos = GameManager.Instance.player2.transform.position;
        Vector3 MyPos = transform.position;

        float dirX = PlayerPos.x - MyPos.x;
        float dirY = PlayerPos.y - MyPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX < 0 ? -1 : 1;
        dirY = dirY < 0 ? -1 : 1;

        Vector3 PlayerDir;

        if (id == 1)
            PlayerDir = GameManager.Instance.player1.InputVec;
        else
            PlayerDir = GameManager.Instance.player2.InputVec;
        switch (transform.tag)
        {
            case "Map":
                if(diffX > diffY)
                {
                    transform.Translate(60 * dirX * Vector3.right);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(60 * dirY * Vector3.up);
                }
                else
                {
                    transform.Translate(dirX * 60, dirY * 60, 0);
                }
                break;
            case "Enemy":
                if(coll.enabled)
                {
                    transform.Translate(PlayerDir * 30 + new Vector3(Random.Range(-2f,2f), Random.Range(-2f, 2f),0f));
                }
                break;
        }
        
    }
}
