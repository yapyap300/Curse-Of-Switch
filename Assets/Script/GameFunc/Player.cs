using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    PlayerInput myInput;
    public Vector2 InputVec;
    public Scanner scanner;
    [SerializeField] WeaponManager[] myWeapon;
    [Header("# Player Info")]
    public float health;
    public int maxHealth;
    [SerializeField] float MoveSpeed;
    [SerializeField] float armor;    
    public int[] statScore;
    public float hitCount;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        myInput = GetComponent<PlayerInput>();
        health = maxHealth;
    } 

    void FixedUpdate()
    {
        if (GameManager.Instance.isStop)
            return;
        Vector2 NextVec = MoveSpeed * Time.fixedDeltaTime * InputVec;
        rigid.MovePosition(rigid.position + NextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed",InputVec.magnitude);

        if (GameManager.Instance.isStop)
            return;
        if(InputVec.x != 0)
        {
            spriter.flipX= InputVec.x < 0;
        }
    }

    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    health -= Time.deltaTime * (10 - armor);

    //    if (GameManager.Instance.isCurse)
    //        hitCount += Time.deltaTime;
    //    if (health < 0)
    //    {
    //        Dead();
    //    }
    //}    

    public void Heal()
    {
        health = maxHealth;
    }
    public void StatUp(int index)//스탯을 public으로 선언하기 싫어서 함수로만듬
    {
        if (statScore[index] == GameManager.Instance.maxStateLevel) return;
        statScore[index]++;
        switch (index)
        {
            case 0:
                armor += 0.5f;                
                break;
            case 1:
                MoveSpeed += 0.2f;
                break;
            case 2:
                maxHealth += 20;
                Heal();
                break;
            case 3:                
                for(int i = 0; i < myWeapon.Length; i++)
                    myWeapon[i].PlusDamage();
                break;
        }        
    }
    public void StatDown(int index)
    {
        if (statScore[index] == 0) return;
        statScore[index]--;
        switch (index)
        {
            case 0:
                armor -= 0.5f;
                break;
            case 1:
                MoveSpeed-= 0.2f;
                break;
            case 2:
                maxHealth -= 20;
                if(health > maxHealth)
                    health = maxHealth;
                break;
            case 3:                
                for (int i = 0; i < myWeapon.Length; i++)
                    myWeapon[i].MinusDamage();
                break;
        }
    }
    public void ChangeKey(string name)
    {
        myInput.SwitchCurrentActionMap(name);
    }
    public void EndingChangeState()
    {
        spriter.DOColor(Color.black, 2f);
        MoveSpeed = 0.5f;
        for(int index = 0; index<myWeapon.Length; index++)
        {
            myWeapon[index].gameObject.SetActive(false);
        }
    }
    void Dead()
    {
        for (int index = 2; index < transform.childCount; index++)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }
        anim.SetTrigger("Dead");
        GameManager.Instance.GameOver();
    }
}
