using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 from = new Vector3( 10.0f,  10.0f, 0.0f);
    public Vector3 to   = new Vector3(-10.0f, -10.0f, 0.0f);

    private float seed = 0;

    private void Awake()
    {
        seed = Random.Range(float.Epsilon, 1);
    }

    private void Update()
    {
        seed += Time.deltaTime;

        Quaternion alpha = Quaternion.Euler(from);
        Quaternion beta  = Quaternion.Euler(to);

        float lerp = 0.5f * (1.0f + Mathf.Sin(Mathf.PI * seed));
        transform.localRotation = Quaternion.Lerp(alpha, beta, lerp);
    }
}