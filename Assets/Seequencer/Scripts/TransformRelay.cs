using System;
using UnityEngine;

public class TransformRelay : MonoBehaviour
{
    public Transform target;

    public bool relayPosition = true;
    public bool lockPositionX = false;
    public bool lockPositionY = false;
    public bool lockPositionZ = false;

    public bool relayRotation = true;
    public bool lockRotationX = false;
    public bool lockRotationY = false;
    public bool lockRotationZ = false;

    public bool relayScale = true;
    public bool lockScaleX = false;
    public bool lockScaleY = false;
    public bool lockScaleZ = false;

    private Transform transform { get { return gameObject.transform; } }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogException(new NullReferenceException(target.name), this);
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (relayPosition)
        {
            gameObject.transform.position = new Vector3(
                lockPositionX ? transform.position.x : target.position.x,
                lockPositionY ? transform.position.y : target.position.y,
                lockPositionZ ? transform.position.z : target.position.z
            );
        }

        if (relayRotation)
        {
            gameObject.transform.rotation = new Quaternion(
                lockRotationX ? transform.rotation.x : target.rotation.x,
                lockRotationY ? transform.rotation.y : target.rotation.y,
                lockRotationZ ? transform.rotation.z : target.rotation.z,
                target.rotation.w
            );
        }

        if (relayScale)
        {
            gameObject.transform.position = new Vector3(
                lockScaleX ? transform.localScale.x : target.localScale.x,
                lockScaleY ? transform.localScale.y : target.localScale.y,
                lockScaleZ ? transform.localScale.z : target.localScale.z
            );
        }
    }
}
