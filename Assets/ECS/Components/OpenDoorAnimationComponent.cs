using UnityEngine;

public enum DoorSate
{
    Stop,
    Opening,
    Open
}

public struct OpenDoorAnimationComponent
{
    public const float OPEN_DOOR_SPEED = 20f;
    
    public int index;
    public Quaternion startRotation;
    public DoorSate doorState;
    public float currentRotateAngle;
}
