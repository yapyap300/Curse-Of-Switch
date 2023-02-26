using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("# Game Object")]
    public Player Player;
    public PoolsManager pool;
    [Header("# Player Info")]
    public int level;
    public int maxLevel;
    public int health;
    public int maxHealth;
    public int exp;
    public int[] nextExp = {10,30,50,70,140,280,560,1800,5400,15000};
    [Header("# Game Control")]
    public float gameTime;
    public float maxTime;
    

    void Awake()
    {
        Instance= this;
        maxTime = 20f * 60f;
        health = maxHealth;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxTime)
        {
            gameTime = maxTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            exp = 0;
            level++;
        }
    }
}
