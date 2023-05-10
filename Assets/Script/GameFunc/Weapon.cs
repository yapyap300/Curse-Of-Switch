using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public int per;
    bool isTrack;//원거리무기중에 유도인지 일반인지 구별하기위해
    int id;
    Collider2D col;
    Rigidbody2D rigid;
    Animator animator;
    Scanner scanner;//유도기능에 사용될 스크립트
    Rigidbody2D target;//유도무기에 사용될 타겟
    Vector3 targetPos;//폭발무기에 쓸 랜덤타겟위치

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    public void Init(float damage, int per, Vector3 dir, int id)//무기마다 초기화를 구분해주기 힘들어서 id 추가
    {
        this.id = id;
        this.damage = damage;
        this.per = per;

        switch (id)
        {
            case 0:
                break;
            case 1:
                animator.SetTrigger("onEnable");
                break;
            case 2:                
                rigid.AddForce(transform.up * 550f, ForceMode2D.Impulse);
                rigid.AddTorque(750f);
                break;
            case 3:
                rigid.velocity = dir * 10f;
                break;
            case 4:
                isTrack = true;
                target = GameManager.Instance.player2.scanner.nearTarget.GetComponent<Rigidbody2D>();
                break;
            case 5:
                col.enabled = true;
                targetPos = dir;
                rigid.velocity = (dir - transform.position).normalized * 10f;
                break;
        }
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.isStop)
            return;
        
        if (isTrack)
            Tracking();
        if (id == 5 && col.enabled == true)      
            End();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)//적이 아니거나 근접무기거나 폭발무기여서 관통력이 원래 -1이라면 무시
            return;
        per--;//원거리총알의 관통력감소

        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            isTrack = false;
            gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || (id != 2 && id != 3))
            return;
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
    void FinishAttack()//애니메이션을 이용한 공격을 비활성화 시키기위해 애니메이션에서 실행할 메서드
    {
        gameObject.SetActive(false);
    }

    void Tracking()//유도무기 전용
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

    void End()//폭발무기 전용 폭발무기는 목표위치로만 이동해서 폭발 애니메이션을 발동시켜야됨
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance < 0.1f)
        {
            rigid.velocity = Vector2.zero;
            col.enabled = false;
            animator.SetTrigger("Boom");
            SoundManager.Instance.PlaySfx("Boom");
        }
    }
}
