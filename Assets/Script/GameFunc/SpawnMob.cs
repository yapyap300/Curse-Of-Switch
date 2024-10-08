using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnMob : MonoBehaviour
{
    [SerializeField] Transform[] spawnPosition;
    public SpawnData[] spawnDatas;
    [SerializeField] Transform[] ground;
    [SerializeField] int level;
    [SerializeField] int eventCount;
    [SerializeField] bool second;//player 1의 스포너인지 2의 스포너인지 구분
    [SerializeField] int[] nextMonster;//다음 레벨의 몬스터소환 시간
    bool drakness;

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
        if (GameManager.Instance.isStop)
            return;
        if (!drakness && level > 4)
        {
            drakness = true;
            SoundManager.Instance.PlayBGM(1);
            foreach(Transform t in ground)
            {
                t.GetComponent<Tilemap>().color = new Color(1f, 0.3f, 0.4f);                
            }
        }
        if (level != nextMonster.Length && GameManager.Instance.gameTime >= nextMonster[level])
            level++;        
    }
    

    public void StatUp(int randomStat)//스폰시간은 건들기에는 너무 난이도에 예민한 수치라서 안 건들이기로 했다. delegate를 이용하기위해 스탯업의 형태를 통일함
    {        
        foreach (SpawnData data in spawnDatas)
        {
            switch (randomStat)
            {
                case 0:
                    data.health += (int)(data.health / 10);
                    break;
                case 1:
                    data.speed += 0.1f;
                    break;               
            }           
        }
    }
    IEnumerator Spawn()//포인트를 여러개 많이 두고 싶지 않아서 4개만 만든후 좌우상하로 랜덤값을 줘서 스폰했다
    {
        while (true)
        {
            GameObject enemy = PoolsManager.Instance.Get(0);
            enemy.GetComponent<Enemy>().Init(spawnDatas[level],second);
            int ranPos = Random.Range(1, 5);
            if (ranPos == 1 || ranPos == 2)
            {
                Vector3 ranSum = new(Random.Range(-12f, 12f), Random.Range(-1f, 1f), 0);
                enemy.transform.position = spawnPosition[ranPos].position + ranSum;
            }
            else
            {
                Vector3 ranSum = new(Random.Range(-1f, 1f), Random.Range(-12f, 12f), 0);
                enemy.transform.position = spawnPosition[ranPos].position + ranSum;
            }
            yield return new WaitForSeconds(spawnDatas[level].spawnTime);
        }
    }
}
