using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : Player
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skull"))
        {
            health -= 10;
            hitCount++;
        }
    }
    void OnMove1(InputValue Value)//Player1¿ë
    {        
        InputVec = Value.Get<Vector2>();        
    }
}
