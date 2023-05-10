using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickButton : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    void Update()
    {
        if (GameManager.Instance.isLevelUp)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buttons[0].onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buttons[1].onClick.Invoke();
            }
            else if (buttons[2].gameObject.activeSelf && Input.GetKeyDown(KeyCode.Alpha3))
            {
                buttons[2].onClick.Invoke();
            }
        }
    }
}
