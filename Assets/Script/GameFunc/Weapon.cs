using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int per;
    bool isTrack;//���Ÿ������߿� �������� �Ϲ����� �����ϱ�����
    int id;
    Collider2D col;
    Rigidbody2D rigid;
    Animator animator;
    Scanner scanner;//������ɿ� ���� ��ũ��Ʈ
    Rigidbody2D target;//�������⿡ ���� Ÿ��
    Vector3 targetPos;//���߹��⿡ �� ����Ÿ����ġ

    [SerializeField]Player master;
    void Awake()
    {
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    public void Init(float damage, int per, Vector3 dir, int id)//���⸶�� �ʱ�ȭ�� �������ֱ� ���� id �߰�
    {
        this.id = id;
        this.damage = damage;
        this.per = per;

        switch (id)
        {
            case 1:
                animator.SetTrigger("onEnable");
                break;
            case 2:                
                rigid.AddForce(transform.up * 750f, ForceMode2D.Impulse);
                rigid.AddTorque(850f);
                break;
            case 3:
                rigid.velocity = dir * 10f;
                break;
            case 4:
                isTrack = true;
                target = GameManager.Instance.Player2.scanner.nearTarget.GetComponent<Rigidbody2D>();
                break;
            case 5:
                col.enabled = false;
                targetPos = dir;
                rigid.velocity = (dir - transform.position).normalized * 10f;
                break;
        }
    }
    void FixedUpdate()
    {
        if(id == 2 && 20 < Vector3.Distance(transform.position, GameManager.Instance.Player1.transform.position))
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        else if (id > 2 && 20 < Vector3.Distance(transform.position, GameManager.Instance.Player2.transform.position))//�Ѿ� ������Ʈ�� ��ô ���� ������ ���� �ۼ�
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        if (isTrack)
            Tracking();
        if (col.enabled == false)//���߹��⸸ ���ư��� �ݶ��̴��� ��Ȱ��ȭ�ϱ⶧���� �Ÿ��� ����       
            End();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)//���� �ƴϰų� ��������ų� ���߹��⿩�� ������� ���� -1�̶�� ����
            return;
        per--;//���Ÿ��Ѿ��� ����°���

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            isTrack = false;
            gameObject.SetActive(false);
        }
    }

    void FinishAttack()//�ִϸ��̼��� �̿��� ������ ��Ȱ��ȭ ��Ű������ �ִϸ��̼ǿ��� ������ �޼���
    {
        gameObject.SetActive(false);
    }

    void Tracking()//�������� ����
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

    void End()//���߹��� ���� ���߹���� ��ǥ��ġ�θ� �̵��ؼ� ���� �ִϸ��̼��� �ߵ����Ѿߵ�
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance < 0.1f)
        {
            rigid.velocity = Vector2.zero;
            col.enabled = true;
            animator.SetTrigger("Boom");
        }
    }
}
