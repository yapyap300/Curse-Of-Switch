using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : Player
{    
    void OnMove1(InputValue Value)//Player1¿ë
    {        
        InputVec = Value.Get<Vector2>();        
    }
}
