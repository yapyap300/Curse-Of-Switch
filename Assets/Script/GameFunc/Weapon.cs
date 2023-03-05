using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int per;
    public bool isTrack = false;//���Ÿ������߿� �������� �Ϲ����� �����ϱ�����
    Rigidbody2D rigid;
    Animator animator;
    Scanner scanner;//������ɿ� ���� ��ũ��Ʈ
    Rigidbody2D target;//�������⿡ ���� Ÿ��

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    public void Init(float damage,int per,Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per > -1)//���Ÿ��������� �ƴ��� ����
        {
            rigid.velocity = dir * 10f;
        }
        else if (rigid != null)//�ٰŸ������ε� rigid�� �������ִ� ��ô���⸸ �̰��� ����
        {
            rigid.AddForce(transform.up * 750f, ForceMode2D.Impulse);
            rigid.AddTorque(850f);
        }
        if (animator != null)//�ִϸ����ʹ� ��� ���⸸ ����������
            animator.SetTrigger("onEnable");
        if (isTrack)//���� �����϶� ù Ÿ���� �ʱ�ȭ����
        {
            target = GameManager.Instance.Player.scanner.nearTarget.GetComponent<Rigidbody2D>();
        }
    }
    void FixedUpdate()
    {   
        if (20 < Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position))//�Ѿ� ������Ʈ�� ��ô ���� ������ ���� �ۼ�
        {
            rigid.velocity = Vector2.zero;
            isTrack = false;
            gameObject.SetActive(false);
        }
        if (!isTrack)
            return;
        Tracking();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)//���� �ƴϰų� �������⿩�� ������� ���� -1�̶�� ����
            return;
        per--;//���Ÿ��Ѿ��� ����°���

        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            isTrack = false;
            gameObject.SetActive(false);
        }
    }
    
    void FinishAttack()//��� ������ ��Ȱ��ȭ ��Ű������ �ִϸ��̼ǿ��� ������ �޼���
    {
        gameObject.SetActive(false);
    }

    void Tracking()
    {
        if (scanner.nearTarget == null || target == null)//���� �״� ó���� OnTriggerEnter2D���� �Ͼ�⶧���� �߰��� Ÿ���� ������ �ʴ� ��찡 ���ܼ� ������ ���� �ڵ�
            return;

        if (target != scanner.nearTarget.GetComponent<Rigidbody2D>())
        {
            target = scanner.nearTarget.GetComponent<Rigidbody2D>();
            return;
        }

        Vector2 dir = target.position - rigid.position;
        Vector2 nextVec = 10f * Time.fixedDeltaTime * dir.normalized;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
}
