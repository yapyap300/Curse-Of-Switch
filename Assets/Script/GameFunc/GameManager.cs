using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{   
    //GameManager에 있는 변수들은 되도록이면 public으로 쓰는것이 좋을 것 같다.다른 곳에서 이곳을 참조하는 경우가 많아서 이다.
    public static GameManager Instance;
    public PanelDate selectUI;    
    [Header("# Game Object")]
    public Player player1;
    public Player player2;
    public SpawnMob[] EnemyControl;
    public PoolsManager pool;
    public Curse curseState;
    public GameObject uiEnd;
    [Header("# Player Level Info")]
    public int level;
    public int maxLevel;    
    public int maxStateLevel;//올릴 수있는 스탯의 한계치
    public int exp;
    public int[] nextExp;
    [Header("# Game Control")]
    public bool isStop;
    public float gameTime;
    public float maxTime;
    public int[] curseTime;
    public int curseIndex;
    [Header("# Level Control")]
    public GameObject[] levelPanels;
    public GameObject[] weaponList;

    WaitForSeconds curseAlarm = new WaitForSeconds(5f);

    void Awake()
    {
        Instance= this;
        maxTime = 20f * 60f;
        for(int i = 1; i<nextExp.Length; i++)
        {
            nextExp[i] = Mathf.FloorToInt((float)(nextExp[i-1] * 1.2));
        }
    }
    void Start()
    {
        StartCoroutine(AutoExp());        
    }
    void Update()
    {
        if (GameManager.Instance.isStop)
            return;
        gameTime += Time.deltaTime;
        if (gameTime >= curseTime[curseIndex])
        {
            StartCoroutine(CurseStrart());
            curseIndex++;
        }
        if(gameTime > maxTime)//엔딩씬으로 넘어가도록 할 예정
        {
            gameTime = maxTime;
        }
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRutine());
    }
    IEnumerator GameOverRutine()
    {
        isStop = true;

        yield return new WaitForSeconds(0.5f);

        uiEnd.SetActive(true);
        Stop();
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }
    public void GetExp(int gainExp)
    {
        exp += gainExp;

        if (exp >= nextExp[level])
        {
            exp -= nextExp[level];
            LevelUp();
        }
    }    
    void LevelUp()
    {
        Stop();
        level++;
        levelPanels[0].GetComponent<Panel>().SetPanel();
        levelPanels[1].GetComponent<Panel>().SetPanel();
        if (level > 9)
            levelPanels[2].GetComponent<Panel>().SetPanel();
    }
    public void Stop()
    {
        isStop = true;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isStop= false;
        Time.timeScale = 1;
    }
    IEnumerator CurseStrart()
    {
        curseState.CurseAlarm();
        yield return curseAlarm;
        int switchCount = Random.Range(1, 3);
        int caseNumber;
        if (switchCount == 1)
            caseNumber = Random.Range(0, 4);
        else
            caseNumber = Random.Range(0, 6);
        player1.ChangeKey($"Curse{switchCount}{caseNumber}");
        player2.ChangeKey($"Curse{switchCount}{caseNumber}");
        yield return new WaitForSeconds(Random.Range(55f,115f));
        curseState.EndAlarm();
        yield return curseAlarm;
        player1.ChangeKey("MainPlayer");
        player2.ChangeKey("MainPlayer");
    }
    IEnumerator AutoExp()
    {
        while (true)
        {
            GetExp(1);
            yield return new WaitForSeconds(1f);
        }
    }
}
