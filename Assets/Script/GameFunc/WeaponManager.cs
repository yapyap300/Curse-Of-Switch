using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// � �������� �����Ͽ� �ٸ� ����
    [SerializeField] int prefabId;//������Ʈ Ǯ���� ������ ���� ���п�
    [SerializeField] float damage;
    [SerializeField] int count;//�ֺ��� ���� ����� ����, �ֵθ��� ����� �Լ� ȣ�� ���ݿ� ����, ���Ÿ����� 0��°�� ��������� ���    
    [SerializeField] float speed;//ȸ�� �ӵ� �Ǵ� ���Ÿ� ������ �߻�ӵ��� ���
    [SerializeField] int level;//���� ���� ������Ȳ ���������� �پ��ϰ� ���
    public int maxLevel;//���۰����� ������ �ִ� ���۷���
    
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
    void Update()//���Ÿ� ����� update���� �߻��ϴ°� ���� �ڷ�ƾ�� �̿��ϴ°� �ξ� ������ ���Ƽ� start�� �ۼ�
    {
        switch (id)
        {
            case 0:
                transform.Rotate(speed * Time.deltaTime * Vector3.back);
                break;
            
        }
    }

    public void LevelUp(float damage)//��������� �������� ���ݷ°� ���� ���� ���Ÿ������ 0��°�� �����ϰ� ������� �������ϰ� �߻�ӵ��� ����
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

    void Stack()// �ֺ��� ���� ���� ���ݿ� �޼���
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
            weapon.GetComponent<Weapon>().Init(damage,-1,Vector3.zero);// -1�� ��������� ������ �����ϰ� �Ϸ��� �Ѱ�
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
