using UnityEngine;

public struct ButtonTriggerComponent
{
    public Transform transform;
    public bool inTrigger;
    public int index;
    public float GetRadius() => transform.GetComponent<MeshFilter>().mesh.bounds.extents.x;
}
