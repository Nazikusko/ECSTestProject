using UnityEngine;

public struct TransformPositionComponent
{
    private Transform transform;

    public void SetTransform(Transform transform)
    {
        this.transform = transform;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
