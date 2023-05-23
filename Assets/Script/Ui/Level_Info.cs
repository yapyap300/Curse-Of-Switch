using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Info : MonoBehaviour
{
    [SerializeField] Text[] levelText;
    [SerializeField] GameObject uiPanel;
    [SerializeField] Button resume;
    [SerializeField] Image skull;
    [SerializeField] Text CurseCount;
    void Update()
    {
        if (!GameManager.Instance.isLevelUp && !GameManager.Instance.isCurse && Input.GetKeyDown(KeyCode.Escape)){
            if (GameManager.Instance.isStop)
            {                
                resume.onClick.Invoke();
            }
            else
            {
                GameManager.Instance.Stop();
                CurrentStat();
                SoundManager.Instance.PlaySfx("PauseIn");
                uiPanel.SetActive(true);
            }
        }
    }

    void CurrentStat()
    {
        int level;
        for(int i = 0; i < 6; i++)
        {
            level = GameManager.Instance.weaponList[i].GetComponent<WeaponManager>().level;
            levelText[i].text = GameManager.Instance.weaponList[i].GetComponent<WeaponManager>().isMax ? $"Lv.Max":$"Lv.{level}";
        }
        for(int i = 0; i < 8; i++)
        {
            if(i < 4)
            {
                level = GameManager.Instance.player1.statScore[i];
                levelText[i + 6].text = level == GameManager.Instance.maxStateLevel ? $"Lv.Max":$"Lv.{level}";
            }
            else
            {
                level = GameManager.Instance.player2.statScore[i % 4];
                levelText[i + 6].text = level == GameManager.Instance.maxStateLevel ? $"Lv.Max" : $"Lv.{level}";
            }
        }
        float hitCount = GameManager.Instance.player1.hitCount + GameManager.Instance.player2.hitCount;
        skull.fillAmount = hitCount / 40;
        CurseCount.text = $"{hitCount}";
    }
}
