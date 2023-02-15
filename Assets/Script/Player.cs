using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 InputVec;
    [SerializeField] float MoveSpeed;
    Rigidbody2D Rigid;
    // Start is called before the first frame update
    void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputVec.x = Input.GetAxis("Horizontal");
        InputVec.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 NextVec = InputVec.normalized * MoveSpeed * Time.fixedDeltaTime;
        Rigid.MovePosition(Rigid.position + NextVec);
    }
}
