using UnityEngine;

[CreateAssetMenu]
public class DorInitData : ScriptableObject
{
    public GameObject dorPrefab;
    public float defaultRotateSpeed = 10f;

    public static DorInitData LoadFromAsset()
    {
        return Resources.Load("Data/DorInitData") as DorInitData;
    }
}