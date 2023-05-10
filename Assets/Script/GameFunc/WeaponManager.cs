using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// 어떤 무기인지 구분하여 다른 동작
    [SerializeField] int prefabId;//오브젝트 풀에서 가져올 무기 구분용
    [SerializeField] float damage;// 기본 데미지
    [SerializeField] float plusDamage;//플레이어의 스탯으로부터 추가되는 데미지
    [SerializeField] int count;//주변을 도는 무기와 투척무기는 갯수, 원거리무기 0번째는 관통력으로 사용    
    [SerializeField] float speed;
    public int level;//현재 무기 레벨상황 스탯증가에 다양하게 사용
    [SerializeField] int maxLevel;
    public bool isMax;

    [SerializeField] Player player;
    
    void Awake()
    {
        Init();
    }
    void OnEnable()
    {
        switch (id)
        {            
            case 0:
                StartCoroutine(StackRoutine());
                break;
            case 1:
                StartCoroutine(Stap());
                break;
            case 2:
                StartCoroutine(Throw());
                break;
            case 3:
                StartCoroutine(Fire());
                break;
            case 4:
                StartCoroutine(Fire());
                break;
            case 5:
                StartCoroutine(Boom());
                break;
        }
    }
    
    void Update()//첫번째 근거리 무기만 계속 일정하게 돌면되기때문에 여기서 동작
    {
        if (GameManager.Instance.isStop)
            return;
        if (id == 0)
            transform.Rotate(speed * Time.deltaTime * Vector3.back);
    }

    public void LevelUp()//근접무기는 레벨업시 공격력과 갯수 증가 원거리무기는 0번째를 제외하고 관통력은 증가안하고 발사속도만 증가
    {
        if (isMax) return;                
        level++;
        switch (id)//원래 데미지를 2배씩 증가였는데 난이도 조절이 힘들어서 각자 무기마다 정해진 데미지가 증가하게함
        {
            case 0:
                damage += 5;
                count += 2;
                speed += 5;
                Stack();
                break;
            case 1:
                damage += 10;
                break;
            case 2:
                damage += 15;
                count++;
                speed -= 1f;
                break;
            case 3:
                damage += 3;
                if(level > 5)//기본 미사일은 관통이 5 이상이면 의미가 없는거 같아서 데미지가 더 올라가게 수정
                    damage += 3;               
                else
                    count++;
                speed -= 0.1f;
                break;
            case 4:
                damage += 6;
                speed -= 0.1f;
                break;
            case 5:
                damage += 20;
                speed -= 1f;
                break;
        }
        if (level == 1)       
            gameObject.SetActive(true);
        if (level == maxLevel)
        {
            if(id == 0)
            {
                StopCoroutine(StackRoutine());
                Stack();
            }
            isMax = true;
        }
            
    }
    public void LevelDown()
    {
        if(level == 0) return;        
        level--;        
        switch (id)
        {
            case 0:
                damage -= 5;
                count -= 2;
                speed -= 5;
                StackInit();
                Stack();
                break;
            case 1:
                damage -= 10;
                break;
            case 2:
                damage -= 15;
                speed += 1f;
                count--;
                break;
            case 3:
                damage -= 3;
                speed += 0.1f;
                if (level >= 5)
                    damage -= 3;
                else
                    count--;   
                break;
            case 4:
                damage -= 6;
                speed += 0.1f;
                break;
            case 5:
                damage -= 20;
                speed += 1f;
                break;
        }
        if (level == maxLevel - 1)
        {
            if (id == 0)
            {
                StartCoroutine(StackRoutine());
            }
            isMax = false;
        }        
        if(level == 0)
            gameObject.SetActive(false);
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 90;                
                break;
            case 1:
                speed = 1f;
                break;
            case 2:
                speed = 11f;
                break;
            case 3:
                speed = 1.2f;
                break;
            case 4:
                speed = 1.1f;
                break;
            case 5:
                speed = 11f;
                break;
        }
    }
    public void Init(int level)//히든 스테이지에서 사용될 함수 전 씬에서 최종으로 달성한 레벨을 기준으로 무기들을 구성하는 함수
    {
        this.level = level;
        switch (id)
        {
            case 0:
                damage = level * 5;
                count = level * 2;
                speed += level * 5;                
                break;
            case 1:
                damage = level * 10;
                break;
            case 2:
                damage = level * 15;
                speed -= level * 1f;
                count = level;
                break;
            case 3:
                damage = level * 3;
                speed -= level * 0.1f;
                if (level > 5)
                {
                    damage += (level - 5) * 3;
                    count = 5;
                }
                else
                    count = level;
                break;
            case 4:
                damage = level * 6;
                speed -= level * 0.1f;
                break;
            case 5:
                damage = level * 20;
                speed -= level * 1f;
                break;
        }
    }
    public void PlusDamage()
    {
        plusDamage++;
        if (id == 0)
        {
            StackInit();
            Stack();
        }
    }
    public void MinusDamage()
    {
        plusDamage--;
        if (id == 0)
        {
            StackInit();
            Stack();
        }
    }
    void Stack()// 주변을 도는 근접 공격용 메서드
    {
        for(int index = 0; index < count; index++)
        {
            Transform weapon;
            if(index < transform.childCount)
            {
                weapon = transform.GetChild(index);
            }
            else
            {
                weapon = GameManager.Instance.pool.Get(prefabId).transform;
                weapon.parent = transform;
            }            
            weapon.gameObject.SetActive(true);
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector3 rotVec = 360 * index * Vector3.forward / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 3f,Space.World);
            weapon.GetComponent<Weapon>().Init(damage + plusDamage * level,-1,Vector3.zero,id);// -1은 근접무기는 무조건 관통하게 하려고 한것
        }
    }
    void StackInit()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    IEnumerator StackRoutine()
    {
        while (true)
        {
            Stack();
            yield return new WaitForSeconds(5 + 0.1f * level);
            StackInit();
            yield return new WaitForSeconds(2 - 0.1f * level);
        }
    }
    IEnumerator Stap()//캐릭터가 진행하는 방향으로 무기를 찌르는 메서드 가만히 서있으면 오른쪽을 찌름
    {        
        while (true)
        {
            yield return new WaitForSeconds(speed);
            Transform weapon = GameManager.Instance.pool.Get(prefabId).transform;
            if (weapon.parent != transform)
                weapon.parent = transform;
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector2 rotVec = GameManager.Instance.player1.InputVec;
            if (rotVec.magnitude == 0)
                rotVec = Vector2.right;
            transform.rotation = Quaternion.LookRotation(Vector3.forward,rotVec);//무기 프리펩이아니라 프리펩의 부모인 관리오브젝트 자체가 돌아야함         
            weapon.GetComponent<Weapon>().Init(damage + plusDamage * level, -1, Vector3.zero,id);        
        }
    }
    IEnumerator Throw()//위쪽 랜덤한 각도로 낫을 투척하는 메서드
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            for (int index = 0; index < count; index++)
            {
                Transform weapon = GameManager.Instance.pool.Get(prefabId).transform;
                weapon.SetPositionAndRotation(transform.position, Quaternion.identity);
                Vector3 rotVec = new(0f,0f,Random.Range(-45f,45f));
                weapon.Translate(weapon.up * 1.5f, Space.World);
                weapon.Rotate(rotVec);
                weapon.GetComponent<Weapon>().Init(damage + plusDamage * level, -1, Vector3.zero,id);
                SoundManager.Instance.PlaySfx("Melee0");
                yield return new WaitForSeconds(speed/count);
            }            
        }
    }
    IEnumerator Fire() // 하다보니 원거리 무기들의 하자가 너무 심해서 레벨마다 하나씩 더 발사하게함 몹이 쌓이면 처리가 아예 안됨
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
            for (int index = 0; index < (level/2) + (level%2); index++)
            {
                if (player.scanner.nearTarget != null)
                {
                    Vector3 targetPos = player.scanner.nearTarget.position;
                    Vector3 dir = targetPos - transform.position;
                    dir = dir.normalized;

                    Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

                    bullet.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));
                    bullet.GetComponent<Weapon>().Init(damage + plusDamage * level, count, dir, id);

                    SoundManager.Instance.PlaySfx("Range");
                }
                yield return new WaitForSeconds(0.1f);
            }        
        }
    }
    IEnumerator Boom()//폭발 무기는 화염구를 날아간 경로와 폭발자리에 데미지를 준다.타겟은 랜덤
    {
        while (true)
        {            

            Vector3 targetPos = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-7f, 7f), 0f);
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;

            bullet.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));
            bullet.GetComponent<Weapon>().Init(damage + plusDamage * level, -1, targetPos, id);

            yield return new WaitForSeconds(speed);
        }
    }
}
