using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 from = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 to   = new Vector3(0.0f, 0.0f, 0.0f);

    private float localTime = 0;

    private void Awake()
    {
        /* Uses a random seed to displace the time. */
        localTime = Random.Range(float.Epsilon, 1.0f);
    }

    private void Update()
    {
        /* Increments local time. */
        localTime += Time.deltaTime;

        transform.localRotation = Quaternion.Lerp( /* Linear Interpolation. */
            Quaternion.Euler(from),                         /* Alpha. */
            Quaternion.Euler(to),                           /* Beta. */
            0.5f * (1.0f + Mathf.Sin(Mathf.PI * localTime)) /* Delta. */
        );
    }
}