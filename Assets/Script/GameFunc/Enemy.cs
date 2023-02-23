using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    [SerializeField] float speed;
    [SerializeField] float maxHealth;
    public RuntimeAnimatorController[] controller;
    [SerializeField] Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter= GetComponent<SpriteRenderer>();
        
    }
    void FixedUpdate()// 플레이어 추적
    {
        if (!isLive)
            return;
        Vector2 dir = target.position - rigid.position;
        Vector2 nextVec = speed * Time.fixedDeltaTime * dir.normalized;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;        
    }
    void OnEnable()
    {
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }
    public void Init(SpawnData data)//몬스터 프리펩을 하나만 두고 스탯과 겉모습만 난이도에 맞춰 초기화해준다.
    {
        animator.runtimeAnimatorController = controller[data.spriteType];
        speed = data.speed;
        health = data.health;
        maxHealth = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon"))
            return;

        health -= collision.GetComponent<Weapon>().damage;

        if(health > 0)
        {
            // hit 
        }
        else
        {
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
