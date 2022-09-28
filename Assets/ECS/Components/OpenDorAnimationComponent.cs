using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OpenDorAnimationComponent
{
    public int index;
    public Transform transform;
    public Quaternion startRotation;
    public bool tryPushToOpen;
    public bool isOpened;
    public float currentRotateAngle;
    public const float openDorSpeed = 10f;
}
