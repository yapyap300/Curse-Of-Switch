using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    private static PoolsManager instance;
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    public static PoolsManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
        pools = new List<GameObject>[prefabs.Length];
        
        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
