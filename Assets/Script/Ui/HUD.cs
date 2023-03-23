using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType {Exp,Level,Time,Health1,Health2}
    public InfoType type;

    Text myText;
    Slider mySlider;
    Image myImage;

    void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();
        myImage= GetComponent<Image>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[GameManager.Instance.level];
                mySlider.value = curExp/maxExp;
                break;
            case InfoType.Level:
                myText.text = $"Lv.{GameManager.Instance.level:F0}"; 
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.maxTime - GameManager.Instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = $"{min:D2}:{sec:D2}";
                break;
            case InfoType.Health1:
                {
                    float curHealth = GameManager.Instance.Player1.health;
                    float maxHealth = GameManager.Instance.Player1.maxHealth;
                    mySlider.value = curHealth / maxHealth;
                    break;
                }
            case InfoType.Health2:
                {
                    float curHealth = GameManager.Instance.Player2.health;
                    float maxHealth = GameManager.Instance.Player2.maxHealth;
                    mySlider.value = curHealth / maxHealth;
                }
                break;

        }
    }
}
