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
    void FixedUpdate()
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
    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = controller[data.spriteType];
        speed = data.speed;
        health = data.health;
        maxHealth = data.health;
    }
}
