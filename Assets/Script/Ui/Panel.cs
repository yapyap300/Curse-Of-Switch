using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    //delegate를 사용해야 내 생각에 맞는 버튼클릭 함수를 정의할수 있을것 같다. 패널이 활성화 될때마다 다른 동작을 해야하기 때문이다.
    private delegate void PanelDelegate();
    private PanelDelegate panel;
    [SerializeField]int id;

    private void Awake()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(PlayDelegate);//패널을 재사용하기위해 버튼에 레벨업마다 다른 함수를 넣어줘야되서 delegate 사용이 불가피하다.
    }
    public void SetPanel()
    {
        switch (id)
        {
            case 0:
                {
                    panel = null; //레벨업마다 다른 동작을 위해 델리게이트 초기화 다른 패널함수도 동일
                    int count = 0;
                    for (int index = 0; index < GameManager.Instance.weaponList.Length; index++)
                    {
                        if (GameManager.Instance.weaponList[index].GetComponent<WeaponManager>().isMax)
                            count++;
                    }
                    Image select = transform.Find("Image").GetComponent<Image>();
                    Text name = transform.Find("Name").GetComponent<Text>();
                    Text level = transform.Find("Level").GetComponent<Text>();
                    Text effect = transform.Find("Effect").GetComponent<Text>();
                    Button upgrade = transform.GetComponentInChildren<Button>();
                    if (count == 6)//모든 무기를 업그레이드 했다면...
                    {
                        select.sprite = GameManager.Instance.selectUI.stat[2];//체력 아이콘 사용

                        name.text = "";//사용안함
                        level.text = "";//사용안함
                        effect.text = "회복";

                        panel += GameManager.Instance.player1.Heal;
                        panel += GameManager.Instance.player2.Heal;
                    }
                    else
                    {
                        int randomSelect = Random.Range(0, 6);

                        while (GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().isMax)//레벨이 이미 max인 무기는 업그레이드 할 수 없다. 
                        {
                            randomSelect = Random.Range(0, 6);
                        }
                        select.sprite = GameManager.Instance.selectUI.weapon[randomSelect];

                        name.text = GameManager.Instance.selectUI.weaponName[randomSelect];
                        level.text = $"레벨:{(GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().level + 1 == 10 ? "Max" : GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().level + 1)}";
                        effect.text = GameManager.Instance.selectUI.weaponDetails[randomSelect];

                        panel += GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().LevelUp;
                    }
                    gameObject.SetActive(true);
                }
                break;
            case 1:
                {
                    panel = null;
                    int count = 0;
                    for (int index = 0; index < 4; index++)
                    {
                        if (GameManager.Instance.player1.statScore[index] == GameManager.Instance.maxStateLevel && GameManager.Instance.player2.statScore[index] == GameManager.Instance.maxStateLevel)
                            count++;
                    }
                    Image select = transform.Find("Image").GetComponent<Image>();
                    Text name = transform.transform.Find("Name").GetComponent<Text>();
                    Text level1 = transform.Find("Level 1").GetComponent<Text>();
                    Text level2 = transform.Find("Level 2").GetComponent<Text>();
                    Text effect = transform.Find("Effect").GetComponent<Text>();
                    Button upgrade = transform.GetComponentInChildren<Button>();
                    if (count == 4)//모든 스탯을 업그레이드 했다면...
                    {
                        select.sprite = GameManager.Instance.selectUI.stat[2];//체력 아이콘 사용

                        name.text = "";//사용안함
                        level1.text = "";//사용안함
                        level2.text = "";//사용안함
                        effect.text = "회복";

                        panel += GameManager.Instance.player1.Heal;
                        panel += GameManager.Instance.player2.Heal;
                    }
                    else
                    {
                        int randomSelect = Random.Range(0, 4);

                        while (GameManager.Instance.player1.statScore[randomSelect] == GameManager.Instance.maxStateLevel && GameManager.Instance.player2.statScore[randomSelect] == GameManager.Instance.maxStateLevel)//레벨이 이미 max인 스탯은 업그레이드 할 수 없다. 
                        {
                            randomSelect = Random.Range(0, 4);
                        }
                        select.sprite = GameManager.Instance.selectUI.stat[randomSelect];

                        name.text = GameManager.Instance.selectUI.statName[randomSelect];
                        level1.text = $"레벨:{(GameManager.Instance.player1.statScore[randomSelect] + 1 == 10 ? "Max" : GameManager.Instance.player1.statScore[randomSelect] + 1)}";
                        level2.text = $"레벨:{(GameManager.Instance.player2.statScore[randomSelect] + 1 == 10 ? "Max" : GameManager.Instance.player2.statScore[randomSelect] + 1)}";
                        effect.text = $"{GameManager.Instance.selectUI.statName[randomSelect]}+";

                        panel += () =>
                        {
                            GameManager.Instance.player1.StatUp(randomSelect);
                            GameManager.Instance.player2.StatUp(randomSelect);
                        };
                    }
                    gameObject.SetActive(true);
                }
                break;
            case 2:
                {
                    panel = null;
                    Image select1 = transform.Find("Image").GetComponent<Image>();
                    Image select2 = transform.Find("Image2").GetComponent<Image>();
                    Text effect = transform.Find("Effect").GetComponent<Text>();
                    Button upgrade = transform.GetComponentInChildren<Button>();

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
                                select1.sprite = GameManager.Instance.selectUI.weapon[random1];
                                select2.sprite = GameManager.Instance.selectUI.weapon[random2];
                                effect.text = GameManager.Instance.selectUI.riskDetails[0];
                                panel += () =>
                                {
                                    GameManager.Instance.weaponList[random1].GetComponent<WeaponManager>().LevelUp();
                                    GameManager.Instance.weaponList[random1].GetComponent<WeaponManager>().LevelUp();
                                    GameManager.Instance.weaponList[random2].GetComponent<WeaponManager>().LevelDown();
                                };
                            }
                            break;
                        case 1:
                            {
                                int random = Random.Range(0, 4);//랜덤한 스탯 선택
                                int randomPlayer = Random.Range(0, 2);//누구의 스탯이 오르고 누가 내려갈지 선택
                                if (randomPlayer == 0)
                                {
                                    select1.sprite = GameManager.Instance.selectUI.player[0];
                                    select2.sprite = GameManager.Instance.selectUI.player[1];
                                    panel += () =>
                                    {
                                        GameManager.Instance.player1.StatUp(random);
                                        GameManager.Instance.player1.StatUp(random);
                                        GameManager.Instance.player2.StatDown(random);
                                    };
                                }
                                else
                                {
                                    select1.sprite = GameManager.Instance.selectUI.player[1];
                                    select2.sprite = GameManager.Instance.selectUI.player[0];
                                    panel += () =>
                                    {
                                        GameManager.Instance.player2.StatUp(random);
                                        GameManager.Instance.player2.StatUp(random);
                                        GameManager.Instance.player1.StatDown(random);
                                    };
                                }
                                effect.text = GameManager.Instance.selectUI.riskDetails[1];
                            }
                            break;
                        case 2:
                            {
                                int random = Random.Range(0, 4);//랜덤한 스탯 선택
                                int randomPlayer = Random.Range(0, 2);//누구의 스탯이 오를지
                                int randomMonsterStat = Random.Range(0, 2);
                                select1.sprite = GameManager.Instance.selectUI.player[randomPlayer];
                                select2.sprite = GameManager.Instance.selectUI.monster;
                                effect.text = GameManager.Instance.selectUI.riskDetails[2];
                                if (randomPlayer == 0)
                                {
                                    panel += () =>
                                    {
                                        GameManager.Instance.player1.StatUp(random);
                                        GameManager.Instance.player1.StatUp(random);
                                        GameManager.Instance.enemyControl[0].StatUp(randomMonsterStat);
                                        GameManager.Instance.enemyControl[1].StatUp(randomMonsterStat);
                                    };
                                }
                                else
                                {
                                    panel += () =>
                                    {
                                        GameManager.Instance.player2.StatUp(random);
                                        GameManager.Instance.player2.StatUp(random);
                                        GameManager.Instance.enemyControl[0].StatUp(randomMonsterStat);
                                        GameManager.Instance.enemyControl[1].StatUp(randomMonsterStat);
                                    };
                                }
                            }
                            break;
                    }
                    gameObject.SetActive(true);
                }
                break;
        }
    }

    void PlayDelegate()
    {
        panel();
        GameManager.Instance.levelPanels[0].SetActive(false);
        GameManager.Instance.levelPanels[1].SetActive(false);
        GameManager.Instance.levelPanels[2].SetActive(false);
        SoundManager.instance.PlaySfx("Select");
        GameManager.Instance.isLevelUp = false;
        GameManager.Instance.Resume();
    }
}
