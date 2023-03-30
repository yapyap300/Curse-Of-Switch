using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public Vector2 InputVec;
    public Scanner scanner;
    [SerializeField] WeaponManager[] myWeapon;
    [Header("# Player Info")]
    public float health;
    public int maxHealth;
    [SerializeField] float MoveSpeed;
    [SerializeField] int armor;    
    public int[] statScore;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        health = maxHealth;
    } 

    void FixedUpdate()
    {
        if (GameManager.Instance.isStop)
            return;
        Vector2 NextVec = MoveSpeed * Time.fixedDeltaTime * InputVec;
        rigid.MovePosition(rigid.position + NextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed",InputVec.magnitude);

        if (GameManager.Instance.isStop)
            return;
        if(InputVec.x != 0)
        {
            spriter.flipX= InputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        health -= Time.deltaTime * (10 - armor);

        if (health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
    public void TakeDamage()
    {
        health -= (1 - (armor / 10f));
        if(health >= 0)
            Dead();
    }
    public void Heal()
    {
        health = maxHealth;
    }
    public void StatUp(int index)//½ºÅÈÀ» publicÀ¸·Î ¼±¾ðÇÏ±â ½È¾î¼­ ÇÔ¼ö·Î¸¸µë
    {
        if (statScore[index] == 5) return;
        statScore[index]++;
        switch (index)
        {
            case 0:
                armor++;                
                break;
            case 1:
                MoveSpeed++;
                break;
            case 2:
                maxHealth += 20;
                Heal();
                break;
            case 3:                
                for(int i = 0; i < myWeapon.Length; i++)
                    myWeapon[i].PlusDamage();
                break;
        }        
    }
    public void StatDown(int index)
    {
        if (statScore[index] == 0) return;
        statScore[index]--;
        switch (index)
        {
            case 0:
                armor--;
                break;
            case 1:
                MoveSpeed--;
                break;
            case 2:
                maxHealth -= 20;
                break;
            case 3:                
                for (int i = 0; i < myWeapon.Length; i++)
                    myWeapon[i].MinusDamage();
                break;
        }
    }
    void Dead()//Á×´Â¾ÀÀ¸·Î ³Ñ±â±â
    {

    }    
}
