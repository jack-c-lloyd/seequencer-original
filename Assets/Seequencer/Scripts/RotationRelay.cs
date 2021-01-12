using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRelay : MonoBehaviour
{
    public Transform target;

    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    private void LateUpdate()
    {
        gameObject.transform.rotation = new Quaternion(
            lockX ? gameObject.transform.rotation.x : target.rotation.x,
            lockY ? gameObject.transform.rotation.y : target.rotation.y,
            lockZ ? gameObject.transform.rotation.z : target.rotation.z,
            target.rotation.w
        );
    }
}
