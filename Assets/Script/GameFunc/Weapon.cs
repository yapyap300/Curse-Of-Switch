using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void Init(float damage,int per,Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if(per > -1)
        {
            rigid.velocity = dir * 10f;
        }
        if (animator != null)
            animator.SetTrigger("onEnable");
            
    }
    void FixedUpdate()
    {
        if (20 < Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position))//총알 오브젝트 관리를 위해 작성
            gameObject.SetActive(false);        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)//적이 아니거나 근접무기여서 관통력이 원래 -1이라면 무시
            return;
        per--;//원거리총알의 관통력감소

        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    
    void FinishAttack()//찌르기 공격을 비활성화 시키기위해 애니메이션에서 실행할 메서드
    {
        gameObject.SetActive(false);
    }
}
