using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : Player
{
    void OnMove2(InputValue Value)//Player1��
    {
        InputVec = Value.Get<Vector2>();        
    }
}
