using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// 어떤 무기인지 구분하여 다른 동작
    [SerializeField] int prefabId;//오브젝트 풀에서 가져올 무기 구분용
    [SerializeField] float damage;
    [SerializeField] int count;//주변을 도는 무기는 갯수, 휘두르는 무기는 함수 호출 간격용 변수, 원거리무기 0번째는 관통력으로 사용    
    [SerializeField] float speed;//회전 속도 또는 원거리 무기의 발사속도에 사용
    [SerializeField] int level;//현재 무기 레벨상황 스탯증가에 다양하게 사용
    public int maxLevel;//업글가능한 무기의 최대 업글레벨
    
    Player player;
    void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    void Start()
    {
        Init();
        if (id == 1)
            StartCoroutine(Fire());
    }
    void Update()//원거리 무기는 update에서 발사하는것 보다 코루틴을 이용하는게 훨씬 좋을것 같아서 start에 작성
    {
        switch (id)
        {
            case 0:
                transform.Rotate(speed * Time.deltaTime * Vector3.back);
                break;
            
        }
    }

    public void LevelUp(float damage)//근접무기는 레벨업시 공격력과 갯수 증가 원거리무기는 0번째를 제외하고 관통력은 증가안하고 발사속도만 증가
    {
        this.damage += damage;
        level++;
        switch (id)
        {
            case 0:
                count++;
                Stack();
                break;
            case 1:
                count++;                
                break;
        }
    }
    
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Stack();
                break;
            case 1:
                speed = 1f;
                break;
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
            
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 1.5f,Space.World);
            weapon.GetComponent<Weapon>().Init(damage,-1,Vector3.zero);// -1은 근접무기는 무조건 관통하게 하려고 한것
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            if (player.scanner.nearTarget != null)
            {
                Vector3 targetPos = player.scanner.nearTarget.position;
                Vector3 dir = targetPos - transform.position;
                dir = dir.normalized;

                Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.position = transform.position;
                bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                bullet.GetComponent<Weapon>().Init(damage, count, dir);
            }

            yield return new WaitForSeconds(speed / level);
        }
    }
}
