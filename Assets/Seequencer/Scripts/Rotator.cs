using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 from = new Vector3( 10.0f,  10.0f, 0.0f);
    public Vector3 to   = new Vector3(-10.0f, -10.0f, 0.0f);

    private float time = 0;

    private void Awake()
    {
        time = Random.Range(float.Epsilon, 1.0f);
    }

    private void Update()
    {
        time += Time.deltaTime;

        Quaternion alpha = Quaternion.Euler(from);
        Quaternion beta  = Quaternion.Euler(to);

        float delta = 0.5f * (1.0f + Mathf.Sin(Mathf.PI * time));

        transform.localRotation = Quaternion.Lerp(alpha, beta, delta);
    }
}