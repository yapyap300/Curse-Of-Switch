using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// � �������� �����Ͽ� �ٸ� ����
    [SerializeField] int prefabId;//������Ʈ Ǯ���� ������ ���� ���п�
    [SerializeField] float damage;// �⺻ ������
    [SerializeField] float plusDamage;//�÷��̾��� �������κ��� �߰��Ǵ� ������
    [SerializeField] int count;//�ֺ��� ���� ����� ��ô����� ����, �ֵθ��� ����� �Լ� ȣ�� ���ݿ� ����, ���Ÿ����� 0��°�� ��������� ���    
    [SerializeField] float speed;//ȸ�� �ӵ�, ��� ���ݼӵ�,���Ÿ� ������ �߻�ӵ��� ��� �ʱ⿡ �������� �������� ��������
    public int level;//���� ���� ������Ȳ ���������� �پ��ϰ� ���
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
    void Update()//ù��° �ٰŸ� ���⸸ ��� �����ϰ� ����Ǳ⶧���� ���⼭ ����
    {
        if (level == maxLevel)
            isMax = true;
        if (id == 0)
            transform.Rotate(speed * Time.deltaTime * Vector3.back);
    }

    public void LevelUp()//��������� �������� ���ݷ°� ���� ���� ���Ÿ������ 0��°�� �����ϰ� ������� �������ϰ� �߻�ӵ��� ����
    {
        if (isMax) return;                
        level++;
        switch (id)//���� �������� 2�辿 �������µ� ���̵� ������ ���� ���� ���⸶�� ������ �������� �����ϰ���
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
            case 4://��������� ������ �������� �ʴ� ��� �������� 2������
                damage += 3 * 2;
                break;
            case 5:
                damage += 20;
                break;
        }
        if (level == 1)//���⸦ ó�� Ȱ��ȭ �Ҷ� ���� ������ �������� ������ �ڷ�ƾ�� �ݺ� �ֱ⸦ ����Ҷ� count�� ������ ����ϴ� ������� count�� 0���� ����ؼ� ������ ���        
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
            case 4://��������� ������ �������� �ʴ� ��� �������� 2������
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
            weapon.GetComponent<Weapon>().Init(damage + plusDamage,-1,Vector3.zero,id);// -1�� ��������� ������ �����ϰ� �Ϸ��� �Ѱ�
        }
    }
    IEnumerator Stap()//���ݼӵ��� ���� ĳ���Ͱ� �����ϴ� �������� ���⸦ ��� �޼��� ������ �������� �������� �
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
            transform.rotation = Quaternion.LookRotation(Vector3.forward,rotVec);//���� �������̾ƴ϶� �������� �θ��� ����������Ʈ ��ü�� ���ƾ���         
            weapon.GetComponent<Weapon>().Init(damage + plusDamage, -1, Vector3.zero,id);        
        }

    }
    IEnumerator Throw()//���� ������ ������ ���� ��ô�ϴ� �޼���
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
    IEnumerator Fire()// ���Ÿ� ���Ⱑ �⺻���� �����ϴ� �޼��� ������������ �ƴ��� �����ϴ� �Ķ���� ���
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
    IEnumerator Boom()//���� ����� ������������ ���� �ֵ� ���� ������ ��ǥ��ġ�� ȭ������ ������ ���ڸ����� �����Ҷ� �������� �ش�.
    {
        while (true)
        {            
            Vector3 targetPos = transform.position + new Vector3(Random.Range(-11f,11f), Random.Range(-5f, 5f), 0f);
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
            
            bullet.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.up, dir));            
            bullet.GetComponent<Weapon>().Init(damage + plusDamage, -1, targetPos, id);//���߹���� ���̶� �΋H���� �������� ��������

            yield return new WaitForSeconds(speed / level);
        }
    }
}
