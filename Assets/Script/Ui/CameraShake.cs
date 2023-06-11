using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float time;
    CinemachineImpulseSource mySource;
    Vector3 reverse;
    void Awake()
    {
        mySource= GetComponent<CinemachineImpulseSource>();
        time = 1f;
        reverse = new Vector3(0f, 2f, 0f);
    }

    void OnEnable()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while(true)
        {
            mySource.GenerateImpulse();
            yield return new WaitForSeconds(time);
            time *= 0.8f;
            mySource.GenerateImpulseWithVelocity(reverse);
            yield return new WaitForSeconds(time);
            time *= 0.8f;
        }        
    }
}
