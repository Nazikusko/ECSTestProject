using UnityEngine;

[CreateAssetMenu]
public class PlayerInitData : ScriptableObject
{
    public GameObject playerPrefab;
    public float defaultSpeed;
    public float defaultRotateSpeed = 10f;

    public static PlayerInitData LoadFromAsset()
    {
        return Resources.Load("Data/PlayerInitData") as PlayerInitData;
    }
}
