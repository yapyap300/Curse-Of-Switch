using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosition : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 PlayerPos = GameManager.Instance.Player.transform.position;
        Vector3 MyPos = transform.position;

        float dirX = PlayerPos.x - MyPos.x;
        float dirY = PlayerPos.y - MyPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX < 0 ? -1 : 1;
        dirY = dirY < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Map":
                if(diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 60);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 60);
                }
                else
                {
                    transform.Translate(dirX * 60, dirY * 60, 0);
                }
                break;
        }
        
    }
}
