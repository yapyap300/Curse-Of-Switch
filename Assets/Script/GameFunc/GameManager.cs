using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{   
    //GameManager�� �ִ� �������� �ǵ����̸� public���� ���°��� ���� �� ����.�ٸ� ������ �̰��� �����ϴ� ��찡 ���Ƽ� �̴�.
    public static GameManager Instance;
    public PanelDate selectUI;
    [Header("# Game Object")]
    public Player Player1;
    public Player Player2;
    public SpawnMob[] EnemyControl;
    public PoolsManager pool;
    [Header("# Player Level Info")]
    public int level;
    public int maxLevel;    
    public int maxStateLevel;//�ø� ���ִ� ������ �Ѱ�ġ
    public int exp;
    public int[] nextExp;
    [Header("# Game Control")]
    public float gameTime;
    public float maxTime;
    [Header("# Level Control")]
    public GameObject[] levelPanels;
    public GameObject[] weaponList;
    

    void Awake()
    {
        Instance= this;
        maxTime = 30f * 60f;
        for(int i = 4; i<maxLevel; i++)
        {
            nextExp[i] = Mathf.FloorToInt((float)(nextExp[i-1] * 1.1));
        }
    }
    void Start()
    {
        StartCoroutine(AutoExp());        
    }
    void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxTime)//���������� �Ѿ���� �� ����
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
            LevelUp();
        }
    }

    void LevelUp()
    {
        Time.timeScale = 0;
        level++;
        levelPanels[0].GetComponent<Panel>().SetPanel();
        levelPanels[1].GetComponent<Panel>().SetPanel();
        if (level > 9)
            levelPanels[2].GetComponent<Panel>().SetPanel();
    }

    IEnumerator AutoExp()
    {
        while (true)
        {
            GetExp();
            yield return new WaitForSeconds(1f);
        }
    }
}
