using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolsManager pool;

    public float gameTime;
    public float maxTime = 2 * 10f;
    public Player Player;

    void Awake()
    {
        Instance= this;        
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxTime)
        {
            gameTime = maxTime;
        }
    }
}
