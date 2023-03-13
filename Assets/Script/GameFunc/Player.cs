using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    Collider2D col;
    SpriteRenderer spriter;
    Animator anim;
    public Vector2 InputVec;
    public Scanner scanner;
    [Header("# Player Info")]
    public float health;
    public int maxHealth;
    [SerializeField] float MoveSpeed;
    [SerializeField] int armor;
    [SerializeField] int extraDamage;
    public int[] statScore;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        health = maxHealth;
    } 

    void FixedUpdate()
    {
        Vector2 NextVec = InputVec * MoveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + NextVec);
        rigid.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed",InputVec.magnitude);

        if(InputVec.x != 0)
        {
            spriter.flipX= InputVec.x < 0;
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
                break;
            case 3:
                extraDamage++;
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
                extraDamage--;
                break;
        }
    }
    void Dead()//Á×´Â¾ÀÀ¸·Î ³Ñ±â±â
    {

    }
    void OnMove(InputValue Value)
    {
        InputVec = Value.Get<Vector2>();
    }
}
