using System.Collections;
using UnityEngine;

public class SpawnMob : MonoBehaviour
{
    [SerializeField] Transform[] spawnPosition;
    public SpawnData[] spawnDatas;
    int level = 0;
    bool upgrade = false;

    void Awake()
    {
        spawnPosition= GetComponentsInChildren<Transform>();
    }
    void Start()
    {
        StartCoroutine(Spawn());
    }
    void Update()
    {
        if (!upgrade && GameManager.Instance.gameTime >= 15f * 60f)
            UpgradeStage();
        if(GameManager.Instance.gameTime < 15f * 60f)
            level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / (15f * 60f)), spawnDatas.Length);
        else
            level = Mathf.Min(Mathf.FloorToInt((GameManager.Instance.gameTime - 15f * 60f) / (15f * 60f)), spawnDatas.Length);
    }

    void UpgradeStage()
    {
        upgrade = true;
        foreach(SpawnData data in spawnDatas)
        {
            data.health *= 2;
            data.speed *= 1.5f;
            data.spawnTime *= 0.7f;
        }
    }

    IEnumerator Spawn()//포인트를 여러개 많이 두고 싶지 않아서 4개만 만든후 좌우상하로 랜덤값을 줘서 스폰했다
    {
        while (true)
        {
            GameObject enemy = GameManager.Instance.pool.Get(0);
            enemy.GetComponent<Enemy>().Init(spawnDatas[level]);
            int ranPos = Random.Range(1, 5);
            if (ranPos == 1 || ranPos == 2)
            {
                Vector3 ranSum = new(Random.Range(-15f, 15f), Random.Range(-1f, 1f), 0);
                enemy.transform.position = spawnPosition[ranPos].position + ranSum;
            }
            else
            {
                Vector3 ranSum = new(Random.Range(-1f, 1f), Random.Range(-10f, 10f), 0);
                enemy.transform.position = spawnPosition[ranPos].position + ranSum;
            }
            yield return new WaitForSeconds(spawnDatas[level].spawnTime);
        }
    }
}
[System.Serializable]
public class SpawnData{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
