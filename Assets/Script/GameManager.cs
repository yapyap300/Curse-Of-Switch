using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolsManager pool;
    public Player Player;

    void Awake()
    {
        Instance= this;        
    }
}
