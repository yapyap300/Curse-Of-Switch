using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiddenManager : MonoBehaviour
{
    private static HiddenManager instance;

    public Boss boss;
    public Player1 player1;
    public Player2 player2;
    public WeaponManager[] weaponList;
    public ParticleSystem ending;
    [SerializeField] GameObject gameOver;
    public static HiddenManager Instance { get { return instance; } }
    
    void Start()
    {
        instance = this;
        player1.isHidden = true;
        player2.isHidden = true;
        for(int index = 0; index < weaponList.Length; index++)
        {
            weaponList[index].Init(DataManager.Instance.weaponLevels[index]);
        }        
        player1.InitHiddenScene(DataManager.Instance.player1StatLevels);
        player2.InitHiddenScene(DataManager.Instance.player2StatLevels);
        SoundManager.Instance.PlayBGM(2);
    }

    public void GameOver()
    {
        StartCoroutine(Over());
    }
    public void GameEnd()
    {
        StartCoroutine(Ending());
    }
    IEnumerator Over()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        gameOver.SetActive(true);
        yield return new WaitForSecondsRealtime(7f);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene(0);
    }
    IEnumerator Ending()
    {
        Time.timeScale = 0;
        ending.Play();
        yield return new WaitForSecondsRealtime(7f);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene(0);
    }
}
