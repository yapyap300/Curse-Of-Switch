using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// 어떤 무기인지 구분하여 다른 동작
    [SerializeField] int prefabId;//오브젝트 풀에서 가져올 무기 구분용
    [SerializeField] float damage;// 기본 데미지
    [SerializeField] float plusDamage;//플레이어의 스탯으로부터 추가되는 데미지
    [SerializeField] int count;//주변을 도는 무기와 투척무기는 갯수, 휘두르는 무기는 함수 호출 간격용 변수, 원거리무기 0번째는 관통력으로 사용    
    [SerializeField] float speed;//회전 속도, 찌르는 공격속도,원거리 무기의 발사속도에 사용 초기에 정해지고 변경하지 않을예정
    public int level;//현재 무기 레벨상황 스탯증가에 다양하게 사용
    [SerializeField] int maxLevel;
    public bool isMax;

    [SerializeField] Player player;
    
    void Start()
    {
        Init();               
    }
    void OnEnable()
    {
        switch (id)
        {            
            case 0:
                Stack();
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
        if (level == maxLevel)
            isMax = true;
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
                Stack();
                break;
            case 1:
                damage += 10;
                count++;
                break;
            case 2:
                damage += 25;
                count++;
                break;
            case 3:
                damage += 3;
                count++;
                break;
            case 4://유도무기는 관통이 증가하지 않는 대신 데미지가 2배증가
                damage += 3 * 2;
                break;
            case 5:
                damage += 20;
                break;
        }
        if (level == 1)//무기를 처음 활성화 할때 스탯 증가를 먼저하지 않으면 코루틴의 반복 주기를 계산할때 count를 가지고 계산하는 무기들이 count를 0으로 계산해서 오류를 띄움        
            gameObject.SetActive(true);
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
                Stack();
                break;
            case 1:
                damage -= 10;
                count++;
                break;
            case 2:
                damage -= 25;
                count++;
                break;
            case 3:
                damage -= 3;
                count++;
                break;
            case 4://유도무기는 관통이 증가하지 않는 대신 데미지가 2배증가
                damage -= 3 * 2;
                break;
            case 5:
                damage -= 20;
                break;
        }
        if(level == 0)
            gameObject.SetActive(false);
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 100;                
                break;
            case 1:
                speed = 5f;
                break;
            case 2:
                speed = 10f;
                break;
            case 3:
                speed = 1f;
                break;
            case 4:
                speed = 0.5f;
                break;
            case 5:
                speed = 8f;
                break;
        }
    }
    public void PlusDamage()
    {
        plusDamage++;
    }
    public void MinusDamage()
    {
        plusDamage--;
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
            
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector3 rotVec = 360 * index * Vector3.forward / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 6f,Space.World);
            weapon.GetComponent<Weapon>().Init(damage + plusDamage,-1,Vector3.zero,id);// -1은 근접무기는 무조건 관통하게 하려고 한것
        }
    }
    IEnumerator Stap()//공격속도에 따라 캐릭터가 진행하는 방향으로 무기를 찌르는 메서드 가만히 서있으면 오른쪽을 찌름
    {        
        while (true)
        {
            yield return new WaitForSeconds(speed / count);
            Transform weapon = GameManager.Instance.pool.Get(prefabId).transform;
            if (weapon.parent != transform)
                weapon.parent = transform;
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector2 rotVec = GameManager.Instance.Player1.InputVec;
            if (rotVec.magnitude == 0)
                rotVec = Vector2.right;
            transform.rotation = Quaternion.LookRotation(Vector3.forward,rotVec);//무기 프리펩이아니라 프리펩의 부모인 관리오브젝트 자체가 돌아야함         
            weapon.GetComponent<Weapon>().Init(damage + plusDamage, -1, Vector3.zero,id);        
        }

    }
    IEnumerator Throw()//위쪽 랜덤한 각도로 낫을 투척하는 메서드
    {
        while (true)
        {
            yield return new WaitForSeconds(speed / level);
            for (int index = 0; index < count; index++)
            {
                Transform weapon = GameManager.Instance.pool.Get(prefabId).transform;
                weapon.SetPositionAndRotation(transform.position, Quaternion.identity);
                Vector3 rotVec = new(0f,0f,Random.Range(-45f,45f));
                weapon.Translate(weapon.up * 1.5f, Space.World);
                weapon.Rotate(rotVec);
                weapon.GetComponent<Weapon>().Init(damage + plusDamage, -1, Vector3.zero,id);
                
            }            
        }
    }
    IEnumerator Fire()// 원거리 무기가 기본으로 실행하는 메서드 유도무기인지 아닌지 구별하는 파라메터 사용
    {
        while (true)
        {
            if (player.scanner.nearTarget != null)
            {
                Vector3 targetPos = player.scanner.nearTarget.position;
                Vector3 dir = targetPos - transform.position;
                dir = dir.normalized;

                Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
                
                bullet.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));                
                bullet.GetComponent<Weapon>().Init(damage + plusDamage, count, dir,id);                
            }

            yield return new WaitForSeconds(speed / level);
        }
    }
    IEnumerator Boom()//폭발 무기는 일정간격으로 적이 있든 없든 랜덤한 목표위치로 화염구를 날리고 그자리에서 폭발할때 데미지를 준다.
    {
        while (true)
        {            
            Vector3 targetPos = transform.position + new Vector3(Random.Range(-11f,11f), Random.Range(-5f, 5f), 0f);
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
            
            bullet.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));            
            bullet.GetComponent<Weapon>().Init(damage + plusDamage, -1, targetPos, id);//폭발무기는 적이랑 부딫여서 데미지를 주지않음

            yield return new WaitForSeconds(speed / level);
        }
    }
}
