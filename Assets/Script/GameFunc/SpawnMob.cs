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
    [SerializeField] bool second;//player 1�� ���������� 2�� ���������� ����
    [SerializeField] int[] nextMonster;//���� �ٲٱ�
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
            foreach(Transform t in ground)
            {
                t.GetComponent<Tilemap>().color = new Color(1f, 0.3f, 0.4f);                
            }
        }
        if (GameManager.Instance.gameTime >= nextMonster[level])
            level++;        
    }
    

    public void StatUp(int randomStat)//�����ð��� �ǵ�⿡�� �ʹ� ���̵��� ������ ��ġ�� �� �ǵ��̱�� �ߴ�. delegate�� �̿��ϱ����� ���Ⱦ��� ���¸� ������
    {        
        foreach (SpawnData data in spawnDatas)
        {
            switch (randomStat)
            {
                case 0:
                    data.health += (int)(data.health / 10);
                    break;
                case 1:
                    data.speed += 0.2f;
                    break;               
            }           
        }
    }
    IEnumerator Spawn()//����Ʈ�� ������ ���� �ΰ� ���� �ʾƼ� 4���� ������ �¿���Ϸ� �������� �༭ �����ߴ�
    {
        while (true)
        {
            GameObject enemy = GameManager.Instance.pool.Get(0);
            enemy.GetComponent<Enemy>().Init(spawnDatas[level],second);
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
