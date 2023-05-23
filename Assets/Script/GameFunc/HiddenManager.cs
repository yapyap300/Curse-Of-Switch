using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenManager : MonoBehaviour
{
    private static HiddenManager instance;

    public Player1 player1;
    public Player2 player2;
    public WeaponManager[] weaponList;

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
        
    }

    public void GameOver()
    {

    }
}
