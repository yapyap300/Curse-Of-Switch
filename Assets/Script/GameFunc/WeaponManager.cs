using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int id;// � �������� �����Ͽ� �ٸ� ����
    [SerializeField] int prefabId;//������Ʈ Ǯ���� ������ ���� ���п�
    [SerializeField] float damage;// �⺻ ������
    [SerializeField] float plusDamage;//�÷��̾��� �������κ��� �߰��Ǵ� ������
    [SerializeField] int count;//�ֺ��� ���� ����� ��ô����� ����, ���Ÿ����� 0��°�� ��������� ���    
    [SerializeField] float speed;
    public int level;//���� ���� ������Ȳ ���������� �پ��ϰ� ���
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
    
    void Update()//ù��° �ٰŸ� ���⸸ ��� �����ϰ� ����Ǳ⶧���� ���⼭ ����
    {
        if (GameManager.Instance.isStop)
            return;
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
                if(level > 5)//�⺻ �̻����� ������ 5 �̻��̸� �ǹ̰� ���°� ���Ƽ� �������� �� �ö󰡰� ����
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
    public void Init(int level)//���� ������������ ���� �Լ� �� ������ �������� �޼��� ������ �������� ������� �����ϴ� �Լ�
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
            weapon.gameObject.SetActive(true);
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            Vector3 rotVec = 360 * index * Vector3.forward / count;
            weapon.Rotate(rotVec);
            weapon.Translate(weapon.up * 3f,Space.World);
            weapon.GetComponent<Weapon>().Init(damage + plusDamage * level,-1,Vector3.zero,id);// -1�� ��������� ������ �����ϰ� �Ϸ��� �Ѱ�
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
    IEnumerator Stap()//ĳ���Ͱ� �����ϴ� �������� ���⸦ ��� �޼��� ������ �������� �������� �
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
            transform.rotation = Quaternion.LookRotation(Vector3.forward,rotVec);//���� �������̾ƴ϶� �������� �θ��� ����������Ʈ ��ü�� ���ƾ���         
            weapon.GetComponent<Weapon>().Init(damage + plusDamage * level, -1, Vector3.zero,id);        
        }
    }
    IEnumerator Throw()//���� ������ ������ ���� ��ô�ϴ� �޼���
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
    IEnumerator Fire() // �ϴٺ��� ���Ÿ� ������� ���ڰ� �ʹ� ���ؼ� �������� �ϳ��� �� �߻��ϰ��� ���� ���̸� ó���� �ƿ� �ȵ�
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
    IEnumerator Boom()//���� ����� ȭ������ ���ư� ��ο� �����ڸ��� �������� �ش�.Ÿ���� ����
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
