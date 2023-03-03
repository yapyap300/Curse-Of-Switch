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

        if(per > -1)//���Ÿ��������� �ƴ��� ����
        {
            rigid.velocity = dir * 10f;
        }
        else if(rigid != null)//�ٰŸ������ε� rigid�� �������ִ� ��ô���⸸ �̰��� ����
        {
            rigid.AddForce(transform.up * 750f,ForceMode2D.Impulse);
            rigid.AddTorque(850f);
        }
        if (animator != null)
            animator.SetTrigger("onEnable");
            
    }
    void FixedUpdate()
    {
        if (20 < Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position))//�Ѿ� ������Ʈ�� ��ô ���� ������ ���� �ۼ�
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)//���� �ƴϰų� �������⿩�� ������� ���� -1�̶�� ����
            return;
        per--;//���Ÿ��Ѿ��� ����°���

        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
    
    void FinishAttack()//��� ������ ��Ȱ��ȭ ��Ű������ �ִϸ��̼ǿ��� ������ �޼���
    {
        gameObject.SetActive(false);
    }
}
