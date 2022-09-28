using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OpenDorAnimationComponent
{
    public int index;
    public Transform transform;
    public bool tryPushToOpen;
    public bool isOpened;
    public float currentRotateAngle;
    public float openDorSpeed;
}
