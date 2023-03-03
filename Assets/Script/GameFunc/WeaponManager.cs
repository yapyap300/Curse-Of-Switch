using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// � �������� �����Ͽ� �ٸ� ����
    [SerializeField] int prefabId;//������Ʈ Ǯ���� ������ ���� ���п�
    [SerializeField] float damage;
    [SerializeField] int count;//�ֺ��� ���� ����� ��ô����� ����, �ֵθ��� ����� �Լ� ȣ�� ���ݿ� ����, ���Ÿ����� 0��°�� ��������� ���    
    [SerializeField] float speed;//ȸ�� �ӵ�, ��� ���ݼӵ�,���Ÿ� ������ �߻�ӵ��� ���
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
            StartCoroutine(Stap());
        else if (id == 2)
            StartCoroutine(Throw());
        else if (id == 3 || id == 4 || id == 5)
            StartCoroutine(Fire());
    }
    void OnEnable()// ó������ ��Ȱ��ȭ �����̴ٰ� ���� ���⸦ ������ �� Ȱ��ȭ ��ų �����̴�. �ϴ��� �׽�Ʈ�� ���� start�� ��
    {
        
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
                count += 2;
                Stack();
                break;
            case 3:
                count++;                
                break;
        }
    }
    
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 100;
                Stack();
                break;
            case 1:
                speed = 5;
                break;
            case 2:
                speed = 10;
                break;
            case 3:
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
            Vector3 rotVec = 360 * index * Vector3.forward / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 6f,Space.World);
            weapon.GetComponent<Weapon>().Init(damage,-1,Vector3.zero);// -1�� ��������� ������ �����ϰ� �Ϸ��� �Ѱ�
        }
    }
    IEnumerator Stap()//���ݼӵ��� ���� ĳ���Ͱ� �����ϴ� �������� ���⸦ ��� �޼��� ������ �������� �������� �
    {        
        while (true)
        {
            Transform weapon = GameManager.Instance.pool.Get(prefabId).transform;
            if (weapon.parent != transform)
                weapon.parent = transform;
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector2 rotVec = GameManager.Instance.Player.InputVec;
            if (rotVec.magnitude == 0)
                rotVec = Vector2.right;
            transform.rotation = Quaternion.LookRotation(Vector3.forward,rotVec);//���� �������̾ƴ϶� �������� �θ��� ����������Ʈ ��ü�� ���ƾ���         
            weapon.GetComponent<Weapon>().Init(damage, -1, Vector3.zero);           
            
            yield return new WaitForSeconds(speed / count);
        }

    }
    IEnumerator Throw()//���� ������ ������ ���� ��ô�ϴ� �޼���
    {
        while (true)
        {
            for (int index = 0; index < count; index++)
            {
                Transform weapon = GameManager.Instance.pool.Get(prefabId).transform;
                weapon.SetPositionAndRotation(transform.position, Quaternion.identity);
                Vector3 rotVec = new(0f,0f,Random.Range(-45f,45f));
                weapon.Translate(weapon.up * 1.5f, Space.World);
                weapon.Rotate(rotVec);
                weapon.GetComponent<Weapon>().Init(damage, -1, Vector3.zero);
                
            }
            yield return new WaitForSeconds(speed);
        }
    }

    IEnumerator Fire()// ��� ���Ÿ� ���Ⱑ �⺻���� �����ϴ� �޼���
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
