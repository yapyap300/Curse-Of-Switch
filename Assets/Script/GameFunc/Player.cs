using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 InputVec;
    public Scanner scanner;
    [SerializeField] float MoveSpeed;    
    public Rigidbody2D Rigid;
    SpriteRenderer spriter;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    } 

    void FixedUpdate()
    {
        Vector2 NextVec = InputVec * MoveSpeed * Time.fixedDeltaTime;
        Rigid.MovePosition(Rigid.position + NextVec);
    }
    void LateUpdate()
    {
        anim.SetFloat("Speed",InputVec.magnitude);

        if(InputVec.x != 0)
        {
            spriter.flipX= InputVec.x < 0;
        }
    }
    void OnMove(InputValue Value)
    {
        InputVec = Value.Get<Vector2>();
    }
}
