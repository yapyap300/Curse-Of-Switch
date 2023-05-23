using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    //GameManager�� �ִ� �������� �ǵ����̸� public���� ���°��� ���� �� ����.�ٸ� ������ �̰��� �����ϴ� ��찡 ���Ƽ� �̴�.
    public static GameManager Instance;
    public PanelDate selectUI;    
    [Header("# Game Object")]
    public Player player1;
    public Player player2;
    public SpawnMob[] enemyControl;    
    public Curse curseState;
    public GameObject[] uiEnd;
    [Header("# Player Level Info")]
    public int level;
    public int maxLevel;    
    public int maxStateLevel;//�ø� ���ִ� ������ �Ѱ�ġ
    public int exp;
    public int[] nextExp;
    [Header("# Game Control")]
    public bool isStop;
    public bool isCurse;
    public bool isEnd;
    public bool isLevelUp;
    public float gameTime;
    public float maxTime;
    public int[] curseTime;
    public int curseIndex;
    [Header("# Level Control")]
    public GameObject[] levelPanels;
    public GameObject[] weaponList;

    readonly WaitForSeconds curseAlarm = new(5f);
    readonly WaitForSeconds expUp = new(1);

    void Awake()
    {
        Instance= this;
        maxTime = 20f * 60f;
        nextExp = new int[maxLevel];
        nextExp[0] = 15;
        for(int i = 1; i<nextExp.Length; i++)
        {
            nextExp[i] = Mathf.FloorToInt((float)(nextExp[i-1] * 1.2));
        }
    }
    void Start()
    {
        StartCoroutine(AutoExp());
        SoundManager.Instance.PlayBGM(0);
    }
    void Update()
    {
        if (isStop || isEnd)
            return;
        gameTime += Time.deltaTime;        
        if (curseIndex != curseTime.Length && gameTime >= curseTime[curseIndex])
        {
            StartCoroutine(CurseStrart());
            curseIndex++;
            isCurse = true;
        }
        if(gameTime > maxTime)
        {
            if (player1.hitCount + player2.hitCount <= 40)// ���ֻ����϶� ���� ��Ʈ���� �������� �Ѵ�.
            {
                StartCoroutine(HiddenEnd());
                //���� ������ ������
            }
            StartCoroutine(GameEnd());
        }
    }
    
    public void GameOver()
    {
        StartCoroutine(GameOverRutine());
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
        isLevelUp = true;
        level++;
        SoundManager.Instance.PlaySfx("LevelUp");
        levelPanels[0].GetComponent<Panel>().SetPanel();
        levelPanels[1].GetComponent<Panel>().SetPanel();
        if (level > 9)
            levelPanels[2].GetComponent<Panel>().SetPanel();
        Stop();

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
        SoundManager.Instance.PlaySfx("Curse");
        int switchCount = Random.Range(1, 3);
        int caseNumber;
        if (switchCount == 1)
            caseNumber = Random.Range(0, 4);
        else
            caseNumber = Random.Range(0, 6);
        player1.ChangeKey($"Curse{switchCount}{caseNumber}");
        player2.ChangeKey($"Curse{switchCount}{caseNumber}");
        yield return new WaitForSeconds(Random.Range(25f,55f));
        curseState.EndAlarm();
        yield return curseAlarm;
        SoundManager.Instance.PlaySfx("EndCurse");
        player1.ChangeKey("MainPlayer");
        player2.ChangeKey("MainPlayer");
        isCurse = false;
    }    
    IEnumerator AutoExp()
    {
        while (true)
        {
            yield return expUp;
            GetExp(1);            
        }
    }
    IEnumerator GameEnd()//���ǿ� ���� ������ �׳� �Ϲ� ��������
    {
        isEnd = true;
        uiEnd[1].SetActive(true);
        yield return curseAlarm;
        player1.EndingChangeState();
        player2.EndingChangeState();
    }
    IEnumerator HiddenEnd()//���� �������� ���� �� ����
    {
        isStop = true;
        uiEnd[2].SetActive(true);
        yield return new WaitForSeconds(8f);
        uiEnd[3].SetActive(true);
        yield return new WaitForSeconds(6f);
        int[] weaponLevels = new int[weaponList.Length];
        for(int index = 0; index < weaponList.Length; index++)
        {
            weaponLevels[index] = weaponList[index].GetComponent<WeaponManager>().level;
        }
        int[] player1Stats = new int[4];
        int[] player2Stats = new int[4];
        for(int index = 0; index < 4; index++)
        {
            player1Stats[index] = player1.statScore[index];
            player2Stats[index] = player2.statScore[index];
        }
        DataManager.Instance.SetLevel(weaponLevels, player1Stats, player2Stats);
        SceneManager.LoadScene(2);
    }    
    IEnumerator GameOverRutine()
    {
        isStop = true;

        yield return new WaitForSeconds(0.5f);

        SoundManager.Instance.PlaySfx("Lose");
        uiEnd[0].SetActive(true);
        Stop();
    }
}
