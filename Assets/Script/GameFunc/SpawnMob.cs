using System.Collections;
using UnityEngine;

public class SpawnMob : MonoBehaviour
{
    [SerializeField] Transform[] spawnPosition;
    public SpawnData[] spawnDatas;
    int level = 0;    

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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 300f), spawnDatas.Length);        
    }

    IEnumerator Spawn()//����Ʈ�� ������ ���� �ΰ� ���� �ʾƼ� 4���� ������ �¿���Ϸ� �������� �༭ �����ߴ�
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
