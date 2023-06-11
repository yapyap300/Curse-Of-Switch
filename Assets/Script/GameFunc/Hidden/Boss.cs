using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("# Boss Info")]
    public int maxHelth;
    public float health;
    [SerializeField] float speed;
    public int groggyCount;
    [Header("# Control Object")]
    [SerializeField] GameObject hitPoint;
    [SerializeField] Rigidbody2D[] target;
    [SerializeField] Detect playerDetect;
    [SerializeField] GameObject blackHole;

    Collider2D col;
    Animator anim;
    Rigidbody2D rigid;    
    int targetIndex;
    bool isPlay;
    Vector3 leftScale;
    Vector3 rightScale;
    readonly Vector2 spellPos = new(0.3f, 2.2f);
    readonly WaitForFixedUpdate fix;
    readonly WaitForSeconds change = new(15f);
    readonly WaitForSeconds attackDelay = new(0.3f);
    readonly WaitForSeconds patternDelay = new(5f);
    void Awake()
    {
        leftScale = transform.localScale;
        rightScale = new(leftScale.x * -1, leftScale.y , leftScale.z);
        col = GetComponent<Collider2D>();        
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerDetect = GetComponentInChildren<Detect>();
        health = maxHelth;
    }
    void Start()
    {
        StartCoroutine(ChangeTarget());
        StartCoroutine(Move());
    }
    void Update()
    {
        if (!isPlay && !(groggyCount == 3 || groggyCount == 5) && playerDetect.isDetect)
        {
            anim.SetBool("Walk", false);            
            StartCoroutine(Attack());
        }
        if (!isPlay && groggyCount == 3)
        {
            anim.SetBool("Walk", false);            
            StartCoroutine(TeleportAttack());
        }
        else if (!isPlay && groggyCount == 5)
        {
            anim.SetBool("Walk", false);
            StartCoroutine(CastSpell());
        }        
    }
    void LateUpdate()
    {
        if (target[targetIndex].position.x > rigid.position.x)
            transform.localScale = rightScale;
        else transform.localScale = leftScale;        
    }    
    
    IEnumerator ChangeTarget()
    {
        targetIndex = Random.Range(0, 2);
        yield return change;
    }
    IEnumerator Move()
    {
        anim.SetBool("Walk",true);
        while (!isPlay)
        {
            Vector2 dir = target[targetIndex].position - rigid.position;
            Vector2 nextVec = speed * Time.fixedDeltaTime * dir.normalized;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
            yield return fix;
        }
    }
    IEnumerator Attack()
    {
        isPlay = true;
        yield return attackDelay;
        anim.SetTrigger("Attack");
        SoundManager.Instance.PlaySfx("BossAttack");
        groggyCount++;
        yield return patternDelay;
        isPlay = false;
        StartCoroutine(Move());
    }
    IEnumerator TeleportAttack()//그로기 카운트가 3일때 발동 랜덤으로 이동하여 타겟을 향해 공격
    {
        isPlay = true;
        int count = Random.Range(3, 6);
        for(int index = 0; index < count; index++)
        {
            anim.SetTrigger("Teleport");
            SoundManager.Instance.PlaySfx("BossTeleport");
            yield return new WaitForSeconds(2.3f);
            anim.SetTrigger("Attack");
            SoundManager.Instance.PlaySfx("BossAttack");
            yield return new WaitForSeconds(0.5f);
        }
        groggyCount++;
        yield return patternDelay;
        isPlay = false;
        StartCoroutine(Move());
    }
    IEnumerator CastSpell()//그로기 카운터가 꽉찼을때 발악패턴후 그로기상태로 딜타임
    {
        isPlay = true;
        blackHole.SetActive(true);
        for(int index = 0; index < 20; index++)
        {
            anim.SetTrigger("Cast");
            yield return new WaitForSeconds(0.8f);
            Vector2 pos1 = HiddenManager.Instance.player1.gameObject.transform.position;
            Vector2 pos2 = HiddenManager.Instance.player2.gameObject.transform.position;
            Transform warning1 = PoolsManager.Instance.Get(7).transform;
            Transform warning2 = PoolsManager.Instance.Get(7).transform;
            warning1.position = pos1;
            warning2.position = pos2;
            yield return new WaitForSeconds(0.5f);
            warning1.gameObject.SetActive(false);
            warning2.gameObject.SetActive(false);
            Transform spell1 = PoolsManager.Instance.Get(6).transform;
            Transform spell2 = PoolsManager.Instance.Get(6).transform;
            spell1.position = pos1 + spellPos;
            spell2.position = pos2 + spellPos;
            SoundManager.Instance.PlaySfx("BossSpell");
        }
        blackHole.SetActive(false);
        anim.SetBool("Groggy", true);
        hitPoint.SetActive(true);
        col.enabled = false;
        yield return new WaitForSeconds(10f);
        anim.SetBool("Groggy", false);
        col.enabled = true;
        hitPoint.SetActive(false);
        groggyCount = 0;
        yield return patternDelay;
        isPlay = false;
        StartCoroutine(Move());
    }
    
    void Teleport()//애니메이션에 이벤트로 사용할 함수
    {
        rigid.position = new(Random.Range(-17f, 17f), Random.Range(-10f, 10f));
    }
}
