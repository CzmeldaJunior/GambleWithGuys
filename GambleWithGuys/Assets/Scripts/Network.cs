using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class Network : MonoBehaviour
{
    // Tady bude zadávat Client IP adresu Hosta
    private string ipAddress = "172.20.10.2";
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
                if (transport != null)
                {
                    // Host naslouchá na 0.0.0.0, aby nikdy nenastala chyba socketu
                    transport.SetConnectionData("127.0.0.1", port, "0.0.0.0");
                }
                NetworkManager.Singleton.StartHost();
            }

            if (GUILayout.Button("Client (Připojit se)"))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                if (transport != null)
                {
                    // Client se připojuje na adresu zadanou v textovém poli
                    transport.SetConnectionData(ipAddress, port);
                }
                NetworkManager.Singleton.StartClient();
            }
        }

        GUILayout.EndArea();
    }
}