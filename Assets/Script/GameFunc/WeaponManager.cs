using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
        }
    }

    public void LevelUp(float damage,int count)
    {
        this.damage = damage;
        this.count = count;
    }

    public void Init()
    {
        switch(id) { 
            case 0:
                speed = 150;
                Stack();
                break;
        }
    }

    void Stack()
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
            weapon.GetComponent<Weapon>().Init(damage,-1);// -1은 근접무기는 무조건 관통하게 하려고 한것
        }
    }
}
