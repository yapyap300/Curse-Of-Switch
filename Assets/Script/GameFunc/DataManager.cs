using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    public int[] weaponLevels;
    public int[] player1StatLevels;
    public int[] player2StatLevels;

    public static DataManager Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int[] weapons, int[] player1, int[] player2)
    {
        weaponLevels= weapons;
        player1StatLevels= player1;
        player2StatLevels= player2;
    }
}
