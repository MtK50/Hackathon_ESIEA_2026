using UnityEngine;
using ArduinoBluetoothAPI;
using UnityEngine.Events;

public enum ConnectionState { NotConnected, FailedToConnected, Connected}

public class BluetoothManager : MonoBehaviour
{
    private BluetoothHelper m_BluetoothHelper;

    [Header("Device Configuration")]

    [SerializeField]
    string m_DeviceName;

    [Header("Device Informations")]

    [SerializeField]
    ConnectionState m_ConnectionState;

    [SerializeField]
    string m_BluetoothAddress;

    [SerializeField]
    int m_BluetoothID;

    [SerializeField]

    [HideInInspector] public UnityEvent OnConnectedEvent;
    [HideInInspector] public UnityEvent<string> OnDataSendEvent;
    [HideInInspector] public UnityEvent<string> OnDataReceiveEvent;

    void Start()
    {
        TryToConnect();
    }

    public void TryToConnect()
    {
        m_BluetoothHelper = BluetoothHelper.GetNewInstance(m_DeviceName);
        m_BluetoothHelper.OnConnected += OnConnected;
        m_BluetoothHelper.OnConnectionFailed += OnConnectionFailed;
        m_BluetoothHelper.OnDataReceived += OnDataReceived;

        OnDataSendEvent.AddListener(SendData);

        if (m_BluetoothHelper.IsBluetoothEnabled())
        {
            m_BluetoothHelper.setTerminatorBasedStream("\n");
            m_BluetoothHelper.Connect();
        }
    }

    public void Disconnect()
    {
        if (m_BluetoothHelper != null)
        {
            m_BluetoothHelper.Disconnect();
            Debug.Log($"Disconnect to : {m_DeviceName}");
            m_BluetoothAddress = "";
            m_BluetoothID = 0;
            m_ConnectionState = ConnectionState.NotConnected;
        }
            
    }

    void OnConnected()
    {
        Debug.Log($"Connected to : {m_BluetoothHelper.getDeviceName()}");
        m_BluetoothAddress = m_BluetoothHelper.getBluetoothDevice().DeviceAddress;
        m_BluetoothID = m_BluetoothHelper.getId();
        m_ConnectionState = ConnectionState.Connected;
        m_BluetoothHelper.StartListening();
        OnConnectedEvent.Invoke();
    }

    void OnConnectionFailed()
    {
        Debug.LogError("Connection failed!");
        m_ConnectionState = ConnectionState.FailedToConnected;
    }

    void OnDataReceived()
    {
        string received = m_BluetoothHelper.Read();
        OnDataReceiveEvent.Invoke(received);
    }

    void SendData(string data)
    {
        m_BluetoothHelper.SendData(data);
    }

    void OnDestroy()
    {
        Disconnect();
    }
}
