using UnityEngine;

public class Scanner : MonoBehaviour
{

    [SerializeField] float scanRange;
    [SerializeField] LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearTarget;

    void FixedUpdate()
    {
        if (GameManager.Instance.isStop)
            return;
        targets = Physics2D.CircleCastAll(transform.position,scanRange,Vector2.zero,0,targetLayer);// 내 범위 주변에있는 몬스터만 래이캐스트를 이용해 검색한다. 순서는 충돌이 동시에 일어나기때문에 랜덤
        nearTarget = NearTargeting();
    }
    
    Transform NearTargeting()
    {
        Transform result= null;
        float diff = 50;
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPosition = transform.position;
            Vector3 targetPosition = target.transform.position;
            float curDiff = Vector3.Distance(myPosition, targetPosition);
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return  result;
    }
}
