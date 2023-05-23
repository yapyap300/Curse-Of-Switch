using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EventSpawn : MonoBehaviour
{
    [Header("# Game Object")]
    [SerializeField] Transform[] spawnPosition;
    [SerializeField] Transform[] skulls;
    [Header("# Object Data")]
    [SerializeField] int[] eventTime;
    [SerializeField] int orcTime;
    [SerializeField] int endTime;
    [SerializeField] SpawnData spawnData;
    [SerializeField] bool second;

    int eventCount;
    bool isAngry;
    Vector3[] initialPosition;
    readonly WaitForSeconds moveDelay = new(0.75f);
    readonly WaitForSeconds delay = new(1.5f);
    void Awake()
    {
        skulls = new Transform[4];
        initialPosition = new Vector3[4];
        initialPosition[0] = new(-6.5f, 0);
        initialPosition[1] = new(6.5f, 1);
        initialPosition[2] = new(0, 7);
        initialPosition[3] = new(1, -7);        
    }
    void Update()
    {
        if(eventCount != eventTime.Length && GameManager.Instance.gameTime >= eventTime[eventCount])
        {
            StartCoroutine(SkullPattern());
            eventCount++;
        }
        if(!isAngry && GameManager.Instance.gameTime >= orcTime)
        {
            isAngry = true;
            StartCoroutine(Spawn());
        }
        if(isAngry && GameManager.Instance.gameTime >= endTime)
        {
            StopCoroutine(Spawn());
        }
    }
    IEnumerator SkullPattern()
    {
        skulls[0] = PoolsManager.Instance.Get(8).transform;
        skulls[0].transform.position = transform.position + initialPosition[0];
        yield return moveDelay;
        skulls[0].DOMoveX(skulls[0].position.x + 13f, 1.5f).SetEase(Ease.Linear).OnComplete(() => skulls[0].gameObject.SetActive(false));
        yield return delay;
        skulls[1] = PoolsManager.Instance.Get(8).transform;
        skulls[1].transform.position = transform.position + initialPosition[1];
        yield return moveDelay;
        skulls[1].DOMoveX(skulls[1].position.x - 13f, 1.5f).SetEase(Ease.Linear).OnComplete(() => skulls[1].gameObject.SetActive(false));
        yield return delay;
        skulls[2] = PoolsManager.Instance.Get(9).transform;
        skulls[2].transform.position = transform.position + initialPosition[2];
        yield return moveDelay;
        skulls[2].DOMoveY(skulls[2].position.y - 14f, 1.5f).SetEase(Ease.Linear).OnComplete(() => skulls[2].gameObject.SetActive(false));
        yield return delay;
        skulls[3] = PoolsManager.Instance.Get(9).transform;
        skulls[3].transform.position = transform.position + initialPosition[3];
        yield return moveDelay;
        skulls[3].DOMoveY(skulls[3].position.y + 14f, 1.5f).SetEase(Ease.Linear).OnComplete(() => skulls[3].gameObject.SetActive(false));
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            GameObject enemy = PoolsManager.Instance.Get(0);
            enemy.GetComponent<Enemy>().Init(spawnData, second);
            int ranPos = Random.Range(0, 4);
            if (ranPos == 0 || ranPos == 1)
            {
                Vector3 ranSum = new(Random.Range(-10f, 10f), Random.Range(-1f, 1f), 0);
                enemy.transform.position = spawnPosition[ranPos].position + ranSum;
            }
            else
            {
                Vector3 ranSum = new(Random.Range(-1f, 1f), Random.Range(-10f, 10f), 0);
                enemy.transform.position = spawnPosition[ranPos].position + ranSum;
            }
            yield return new WaitForSeconds(spawnData.spawnTime);
        }
    }
}
