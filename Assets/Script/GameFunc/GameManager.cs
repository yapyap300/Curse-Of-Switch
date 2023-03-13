using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] PanelDate selectUI;
    [Header("# Game Object")]
    public Player Player1;
    public Player Player2;
    public PoolsManager pool;
    [Header("# Player Level Info")]
    public int level;
    public int maxLevel;    
    int maxStateLevel;//올릴 수있는 스탯의 한계치
    public int exp;
    public int[] nextExp = {10,30,50,70,140,280,560,1800,5400,15000};
    [Header("# Game Control")]
    public float gameTime;
    public float maxTime;
    [Header("# Level Control")]
    [SerializeField] GameObject[] levelPanels;
    [SerializeField] GameObject[] weaponList;
    

    void Awake()
    {
        Instance= this;
        maxTime = 30f * 60f;
    }
    void Start()
    {
        //Time.timeScale = 0;
    }
    void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxTime)//엔딩씬으로 넘어가도록 할 예정
        {
            gameTime = maxTime;
        }
    }

    void Panel1()
    {
        int count = 0;
        for (int index = 0; index < weaponList.Length; index++)
        {
            if (weaponList[index].GetComponent<WeaponManager>().isMax)
                count++;
        }
        Image select = levelPanels[0].GetComponentInChildren<Image>();
        Text name = levelPanels[0].transform.Find("Name").GetComponent<Text>();
        Text level = levelPanels[0].transform.Find("Level").GetComponent<Text>();
        Text effect = levelPanels[0].transform.Find("Effect").GetComponent<Text>();
        Button upgrade = levelPanels[0].GetComponentInChildren<Button>();
        if (count == 6)//모든 무기를 업그레이드 했다면...
        {
            select.sprite = selectUI.stat[2];//체력 아이콘 사용

            name.text = "";//사용안함
            level.text = "";//사용안함
            effect.text = "회복";

            upgrade.onClick.AddListener(Player1.Heal);
        }
        else
        {
            int randomSelect = Random.Range(0, 6);

            while (weaponList[randomSelect].GetComponent<WeaponManager>().isMax)//레벨이 이미 max인 무기는 업그레이드 할 수 없다. 
            {
                randomSelect = Random.Range(0, 6);
            }
            select.sprite = selectUI.weapon[randomSelect];

            name.text = selectUI.weaponName[randomSelect];
            level.text = $"레벨:{(weaponList[randomSelect].GetComponent<WeaponManager>().level + 1 == 5 ? "Max" : weaponList[randomSelect].GetComponent<WeaponManager>().level + 1)}";
            effect.text = selectUI.weaponDetails[randomSelect];

            upgrade.onClick.AddListener(weaponList[randomSelect].GetComponent<WeaponManager>().LevelUp);
        }
    }
    void Panel2()
    {
        int count = 0;
        for (int index = 0; index < 4; index++)
        {
            if (Player1.statScore[index] == maxStateLevel)
                count++;
        }
        Image select = levelPanels[1].GetComponentInChildren<Image>();
        Text name = levelPanels[1].transform.Find("Name").GetComponent<Text>();
        Text level = levelPanels[1].transform.Find("Level").GetComponent<Text>();
        Text effect = levelPanels[1].transform.Find("Effect").GetComponent<Text>();
        Button upgrade = levelPanels[1].GetComponentInChildren<Button>();
        if (count == 4)//모든 스탯을 업그레이드 했다면...
        {
            select.sprite = selectUI.stat[2];//체력 아이콘 사용

            name.text = "";//사용안함
            level.text = "";//사용안함
            effect.text = "회복";

            upgrade.onClick.AddListener(Player1.Heal);
        }
        else
        {
            int randomSelect = Random.Range(0, 4);

            while (Player1.statScore[randomSelect] == maxStateLevel)//레벨이 이미 max인 스탯은 업그레이드 할 수 없다. 
            {
                randomSelect = Random.Range(0, 4);
            }
            select.sprite = selectUI.stat[randomSelect];

            name.text = selectUI.statName[randomSelect];
            level.text = $"레벨:{(Player1.statScore[randomSelect]+1 == 5? "Max":Player1.statScore[randomSelect]+1)}";
            effect.text = $"{selectUI.statName[randomSelect]}+";

            upgrade.onClick.AddListener(() => 
            { 
                Player1.StatUp(randomSelect);                
            });
        }
    }
    void Panel3()
    {
        Image select1 = levelPanels[2].transform.Find("Image").GetComponent<Image>();
        Image select2 = levelPanels[2].transform.Find("Image2").GetComponent<Image>();
        Text effect = levelPanels[2].transform.Find("Effect").GetComponent<Text>();
        Button upgrade = levelPanels[2].GetComponentInChildren<Button>();

        int randomSelect = Random.Range(0, 3);
        switch (randomSelect)
        {
            case 0:
                {
                    int random1 = Random.Range(0, 6);
                    int random2 = Random.Range(0, 6);
                    while (random1 == random2)
                    {
                        random2 = Random.Range(0, 6);
                    }
                    select1.sprite = selectUI.weapon[random1];
                    select2.sprite = selectUI.weapon[random2];
                    effect.text = selectUI.riskDetails[0];
                    upgrade.onClick.AddListener(() =>
                    {
                        weaponList[random1].GetComponent<WeaponManager>().LevelUp();
                        weaponList[random1].GetComponent<WeaponManager>().LevelUp();
                        weaponList[random2].GetComponent<WeaponManager>().LevelDown();
                        weaponList[random2].GetComponent<WeaponManager>().LevelDown();
                    });
                }
                break;
            case 1:
                {
                    int random = Random.Range(0, 4);//랜덤한 스탯 선택
                    int randomPlayer = Random.Range(0, 2);//누구의 스탯이 오르고 누가 내려갈지 선택
                    if(randomPlayer == 0)
                    {
                        select1.sprite = selectUI.player[0];
                        select2.sprite = selectUI.player[1];
                        upgrade.onClick.AddListener(() =>
                        {
                            Player1.StatUp(random);
                            Player1.StatUp(random);
                            Player2.StatDown(random);
                            Player2.StatDown(random);
                        });
                    }
                    else
                    {
                        select1.sprite = selectUI.player[1];
                        select2.sprite = selectUI.player[0];
                        upgrade.onClick.AddListener(() =>
                        {
                            Player2.StatUp(random);
                            Player2.StatUp(random);
                            Player1.StatDown(random);
                            Player1.StatDown(random);
                        });
                    }                    
                    effect.text = selectUI.riskDetails[1];                    
                }
                break;
            case 2:
                {
                    int random = Random.Range(0, 4);//랜덤한 스탯 선택
                    int randomPlayer = Random.Range(0, 2);//누구의 스탯이 오를지
                    select1.sprite = selectUI.player[randomPlayer];
                    select2.sprite = selectUI.monster;
                    effect.text = selectUI.riskDetails[2];
                    if (randomPlayer == 0)
                    {
                        upgrade.onClick.AddListener(() =>
                        {
                            Player1.StatUp(random);
                            Player1.StatUp(random);
                        });
                    }
                    else
                    {
                        upgrade.onClick.AddListener(() =>
                        {
                            Player2.StatUp(random);
                            Player2.StatUp(random);
                        });
                    }
                }
                break;            
        }        
    }
    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            exp = 0;
            level++;
        }
    }
}
