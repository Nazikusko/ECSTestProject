using UnityEngine;

public struct TransformRotationComponent
{
    private Transform transform;

    public void SetTransform(Transform transform)
    {
        this.transform = transform;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}