using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public void StartHost()
	{
		var isHostStarted = NetworkManager.Singleton.StartHost();
		Debug.Log(isHostStarted ? "Host Started" : "Host failed to Start");
	}

	public void StartServer()
	{
		var isServerStarted = NetworkManager.Singleton.StartServer();
		Debug.Log(isServerStarted ? "Server Started" : "Server failed to Start");
	}

	public void StartClient()
	{
		var isClientStarted = NetworkManager.Singleton.StartClient();
		Debug.Log(isClientStarted ? "Client Started" : "Client failed to Start");
	}
}