using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class Network : MonoBehaviour
{
    // Tady bude zadávat Client IP adresu Hosta
    private string ipAddress = " 10.107.184.87";
    private ushort port = 7777;

    private void OnGUI()
    {
        if (NetworkManager.Singleton == null) return;

        GUILayout.BeginArea(new Rect(20, 20, 250, 220));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            GUILayout.Label("IP Adresa Hosta (pro Clienta):");
            ipAddress = GUILayout.TextField(ipAddress);

            if (GUILayout.Button("Host (Založit hru)"))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

                transport.SetConnectionData("127.0.0.1", port, "0.0.0.0");

                bool started = NetworkManager.Singleton.StartHost();
                Debug.Log("StartHost: " + started);
            }

            if (GUILayout.Button("Client (Připojit se)"))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

                transport.SetConnectionData(ipAddress, port);

                bool started = NetworkManager.Singleton.StartClient();
                Debug.Log("StartClient: " + started);
            }
        }

        GUILayout.EndArea();
    }
}