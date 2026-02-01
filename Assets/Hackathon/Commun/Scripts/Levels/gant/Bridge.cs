// Bridge.cs
using UnityEngine;

public enum BridgeType
{
    Serial,
    Wifi
}

public class Bridge : MonoBehaviour
{
    public static IBridge Instance { get; private set; }

    [Header("Bridge selection")]
    public BridgeType bridgeType = BridgeType.Wifi;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        switch (bridgeType)
        {
            case BridgeType.Wifi:
                Instance = FindFirstObjectByType<WifiBridge>();
                break;
        }

        if (Instance == null)
        {
            Debug.LogError("[Bridge] Aucun bridge trouvé dans la scène !");
        }
        else
        {
            Debug.Log("[Bridge] Bridge actif : " + bridgeType);
        }
    }
}
