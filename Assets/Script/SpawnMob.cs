using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMob : MonoBehaviour
{
    [SerializeField] Transform[] spawnPosition;

    float Timer;

    private void Awake()
    {
        spawnPosition= GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        Timer += Time.deltaTime;

        if(Timer > 0.3f )
        {
            Timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.pool.Get(Random.Range(0, 4));
        int ranPos = Random.Range(1, 5);
        if(ranPos == 1 || ranPos == 2 )
        {
            Vector3 ranSum = new Vector3(Random.Range(-15f, 15f), Random.Range(-1f, 1f), 0);
            enemy.transform.position = spawnPosition[ranPos].position + ranSum;
        }
        else
        {
            Vector3 ranSum = new Vector3(Random.Range(-1f, 1f), Random.Range(-10f, 10f), 0);
            enemy.transform.position = spawnPosition[ranPos].position + ranSum;
        }        
    }
}
