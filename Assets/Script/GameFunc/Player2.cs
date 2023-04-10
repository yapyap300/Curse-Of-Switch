using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : Player
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skull"))
        {
            health -= 10;
            hitCount++;
        }        
    }
    void OnMove2(InputValue Value)//Player1¿ë
    {
        InputVec = Value.Get<Vector2>();        
    }
}
