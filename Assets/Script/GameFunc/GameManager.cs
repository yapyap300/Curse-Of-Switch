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
    [SerializeField] int maxLevel;
    public int exp;
    [SerializeField] int[] nextExp = {10,30,50,70,140,280,560,1800,5400,15000};
    [Header("# Game Control")]
    public float gameTime;
    public float maxTime = 20 * 60f;
    

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
