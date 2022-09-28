using UnityEngine;

public struct OpenDorAnimationComponent
{
    public const float OPEN_DOR_SPEED = 10f;

    public int index;
    public Transform transform;
    public Quaternion startRotation;
    public bool tryPushToOpen;
    public bool isOpened;
    public float currentRotateAngle;
}
