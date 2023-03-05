using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int per;
    public bool isTrack = false;//원거리무기중에 유도인지 일반인지 구별하기위해
    Rigidbody2D rigid;
    Animator animator;
    Scanner scanner;//유도기능에 사용될 스크립트
    Rigidbody2D target;//유도무기에 사용될 타겟

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

        if (per > -1)//원거리무기인지 아닌지 구별
        {
            rigid.velocity = dir * 10f;
        }
        else if (rigid != null)//근거리무기인데 rigid을 가지고있는 투척무기만 이곳을 실행
        {
            rigid.AddForce(transform.up * 750f, ForceMode2D.Impulse);
            rigid.AddTorque(850f);
        }
        if (animator != null)//애니메이터는 찌르기 무기만 가지고있음
            animator.SetTrigger("onEnable");
        if (isTrack)//유도 무기일때 첫 타겟을 초기화해줌
        {
            target = GameManager.Instance.Player.scanner.nearTarget.GetComponent<Rigidbody2D>();
        }
    }
    void FixedUpdate()
    {   
        if (20 < Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position))//총알 오브젝트와 투척 무기 관리를 위해 작성
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
        if (!collision.CompareTag("Enemy") || per == -1)//적이 아니거나 근접무기여서 관통력이 원래 -1이라면 무시
            return;
        per--;//원거리총알의 관통력감소

        if(per == -1)
        {
            rigid.velocity = Vector2.zero;
            isTrack = false;
            gameObject.SetActive(false);
        }
    }
    
    void FinishAttack()//찌르기 공격을 비활성화 시키기위해 애니메이션에서 실행할 메서드
    {
        gameObject.SetActive(false);
    }

    void Tracking()
    {
        if (scanner.nearTarget == null || target == null)//적이 죽는 처리가 OnTriggerEnter2D에서 일어나기때문에 중간에 타겟이 잡히지 않는 경우가 생겨서 오류를 막는 코드
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
