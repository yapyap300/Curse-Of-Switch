using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    [SerializeField] Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter= GetComponent<SpriteRenderer>();
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (!isLive)
            return;
        Vector2 dir = target.position - rigid.position;
        Vector2 nextVec = dir.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;        
    }
}
