using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    //delegate�� ����ؾ� �� ������ �´� ��ưŬ�� �Լ��� �����Ҽ� ������ ����. �г��� Ȱ��ȭ �ɶ����� �ٸ� ������ �ؾ��ϱ� �����̴�.
    private delegate void PanelDelegate();
    private PanelDelegate panel;
    [SerializeField]int id;

    private void Awake()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(PlayDelegate);//�г��� �����ϱ����� ��ư�� ���������� �ٸ� �Լ��� �־���ߵǼ� delegate ����� �Ұ����ϴ�.
    }
    public void SetPanel()
    {
        switch (id)
        {
            case 0:
                {
                    panel = null; //���������� �ٸ� ������ ���� ��������Ʈ �ʱ�ȭ �ٸ� �г��Լ��� ����
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
                    if (count == 6)//��� ���⸦ ���׷��̵� �ߴٸ�...
                    {
                        select.sprite = GameManager.Instance.selectUI.stat[2];//ü�� ������ ���

                        name.text = "";//������
                        level.text = "";//������
                        effect.text = "ȸ��";

                        panel += GameManager.Instance.Player1.Heal;
                        panel += GameManager.Instance.Player2.Heal;
                    }
                    else
                    {
                        int randomSelect = Random.Range(0, 6);

                        while (GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().isMax)//������ �̹� max�� ����� ���׷��̵� �� �� ����. 
                        {
                            randomSelect = Random.Range(0, 6);
                        }
                        select.sprite = GameManager.Instance.selectUI.weapon[randomSelect];

                        name.text = GameManager.Instance.selectUI.weaponName[randomSelect];
                        level.text = $"����:{(GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().level + 1 == 5 ? "Max" : GameManager.Instance.weaponList[randomSelect].GetComponent<WeaponManager>().level + 1)}";
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
                        if (GameManager.Instance.Player1.statScore[index] == GameManager.Instance.maxStateLevel)
                            count++;
                    }
                    Image select = transform.Find("Image").GetComponent<Image>();
                    Text name = transform.transform.Find("Name").GetComponent<Text>();
                    Text level = transform.Find("Level").GetComponent<Text>();
                    Text effect = transform.Find("Effect").GetComponent<Text>();
                    Button upgrade = transform.GetComponentInChildren<Button>();
                    if (count == 4)//��� ������ ���׷��̵� �ߴٸ�...
                    {
                        select.sprite = GameManager.Instance.selectUI.stat[2];//ü�� ������ ���

                        name.text = "";//������
                        level.text = "";//������
                        effect.text = "ȸ��";

                        panel += GameManager.Instance.Player1.Heal;
                        panel += GameManager.Instance.Player2.Heal;
                    }
                    else
                    {
                        int randomSelect = Random.Range(0, 4);

                        while (GameManager.Instance.Player1.statScore[randomSelect] == GameManager.Instance.maxStateLevel)//������ �̹� max�� ������ ���׷��̵� �� �� ����. 
                        {
                            randomSelect = Random.Range(0, 4);
                        }
                        select.sprite = GameManager.Instance.selectUI.stat[randomSelect];

                        name.text = GameManager.Instance.selectUI.statName[randomSelect];
                        level.text = $"����:{(GameManager.Instance.Player1.statScore[randomSelect] + 1 == 5 ? "Max" : GameManager.Instance.Player1.statScore[randomSelect] + 1)}";
                        effect.text = $"{GameManager.Instance.selectUI.statName[randomSelect]}+";

                        panel += () =>
                        {
                            GameManager.Instance.Player1.StatUp(randomSelect);
                            GameManager.Instance.Player2.StatUp(randomSelect);
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
                                int random = Random.Range(0, 4);//������ ���� ����
                                int randomPlayer = Random.Range(0, 2);//������ ������ ������ ���� �������� ����
                                if (randomPlayer == 0)
                                {
                                    select1.sprite = GameManager.Instance.selectUI.player[0];
                                    select2.sprite = GameManager.Instance.selectUI.player[1];
                                    panel += () =>
                                    {
                                        GameManager.Instance.Player1.StatUp(random);
                                        GameManager.Instance.Player1.StatUp(random);
                                        GameManager.Instance.Player2.StatDown(random);
                                    };
                                }
                                else
                                {
                                    select1.sprite = GameManager.Instance.selectUI.player[1];
                                    select2.sprite = GameManager.Instance.selectUI.player[0];
                                    panel += () =>
                                    {
                                        GameManager.Instance.Player2.StatUp(random);
                                        GameManager.Instance.Player2.StatUp(random);
                                        GameManager.Instance.Player1.StatDown(random);
                                    };
                                }
                                effect.text = GameManager.Instance.selectUI.riskDetails[1];
                            }
                            break;
                        case 2:
                            {
                                int random = Random.Range(0, 4);//������ ���� ����
                                int randomPlayer = Random.Range(0, 2);//������ ������ ������
                                int randomMonsterStat = Random.Range(0, 2);
                                select1.sprite = GameManager.Instance.selectUI.player[randomPlayer];
                                select2.sprite = GameManager.Instance.selectUI.monster;
                                effect.text = GameManager.Instance.selectUI.riskDetails[2];
                                if (randomPlayer == 0)
                                {
                                    panel += () =>
                                    {
                                        GameManager.Instance.Player1.StatUp(random);
                                        GameManager.Instance.Player1.StatUp(random);
                                        GameManager.Instance.EnemyControl[0].StatUp(randomMonsterStat);
                                        GameManager.Instance.EnemyControl[1].StatUp(randomMonsterStat);
                                    };
                                }
                                else
                                {
                                    panel += () =>
                                    {
                                        GameManager.Instance.Player2.StatUp(random);
                                        GameManager.Instance.Player2.StatUp(random);
                                        GameManager.Instance.EnemyControl[0].StatUp(randomMonsterStat);
                                        GameManager.Instance.EnemyControl[1].StatUp(randomMonsterStat);
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
        GameManager.Instance.Resume();
    }
}
