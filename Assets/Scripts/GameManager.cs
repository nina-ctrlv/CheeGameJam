using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private bool _isClient;
    private bool _isClientConfigured;
    private string _clientAddress;
    private UnityTransport _unityTransport;

    private void Start()
    {
        _unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        _clientAddress = _unityTransport.ConnectionData.Address;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!_isClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else if (_isClient && !NetworkManager.Singleton.IsClient)
        {
            ClientConfig();
        }
        else
        {
            StatusLabels();

            // SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    private void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) _isClient = true;
    }

    private void ClientConfig()
    {
        _clientAddress = GUILayout.TextField(_clientAddress);
        _unityTransport.ConnectionData.Address = _clientAddress;
        if (GUILayout.Button("Update Server IP")) NetworkManager.Singleton.StartClient();
    }

    private void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
        GUILayout.Label("Address: " + _unityTransport.ConnectionData.Address);
    }

    static void SubmitNewPosition(GameObject prefab)
    {
    }
}