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
    Collider2D col;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter= GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        wait =  new WaitForFixedUpdate();        
    }
    void FixedUpdate()// 플레이어 추적
    {
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        col.enabled = true;
        rigid.simulated = true;
        animator.SetBool("Dead", false);
        spriter.sortingOrder = 2;
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
        if (!collision.CompareTag("Weapon") || !isLive)
            return;

        health -= collision.GetComponent<Weapon>().damage;
        StartCoroutine(knockBack());

        if(health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            col.enabled = false;
            rigid.simulated = false;
            animator.SetBool("Dead",true);
            spriter.sortingOrder = 1;
            GameManager.Instance.GetExp();
        }
    }
    IEnumerator knockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 dir = transform.position- playerPos;
        rigid.AddForce(dir.normalized * 3,ForceMode2D.Impulse);
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
