// WifiBridge.cs
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;

public class WifiBridge : MonoBehaviour, IBridge
{
    public static WifiBridge Instance;

    [Header("ESP32 WiFi settings")]
    [Tooltip("IP de l'ESP32 (AP = 192.168.4.1)")]
    public string esp32Ip = "192.168.4.1";

    [Tooltip("Port TCP du serveur ESP32")]
    public int port = 3333;

    private TcpClient client;
    private NetworkStream stream;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        try
        {
            client = new TcpClient();
            client.Connect(esp32Ip, port);
            stream = client.GetStream();

            Debug.Log($"[WifiBridge] Connecté à {esp32Ip}:{port}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[WifiBridge] Erreur connexion TCP : {e.Message}");
        }
    }

    /// <summary>
    /// Envoie une string brute (aucun \n ajouté)
    /// </summary>
    public void SendLine(string line)
    {
        if (client == null || !client.Connected || stream == null)
        {
            Debug.LogWarning("[WifiBridge] Non connecté, message non envoyé: " + line);
            return;
        }

        try
        {
            byte[] data = Encoding.ASCII.GetBytes(line);
            stream.Write(data, 0, data.Length);

            Debug.Log("[WifiBridge] envoyé: " + line);
        }
        catch (Exception e)
        {
            Debug.LogError("[WifiBridge] Erreur écriture TCP: " + e.Message);
        }
    }

    void OnDestroy()
    {
        try
        {
            stream?.Close();
            client?.Close();
        }
        catch { }
    }
}
