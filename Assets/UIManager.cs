using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] public TextMeshProUGUI debugText = null;


	public void StartHost()
	{
		if (NetworkManager.Singleton.StartHost())
		{
			Debug.Log("Host Started");
			// debugText = "Host Started";
		}
		else
		{
			Debug.Log("Host failed to Start");
			// debugText = "Host failed to Start";
		}
	}

	public void StartServer()
	{
		if (NetworkManager.Singleton.StartServer())

		{
			Debug.Log("Server Started");

			// debugText = "Server Started";
		}
		else
		{
			Debug.Log("Server failed to Start");

			// debugText = "Server failed to Start";
		}
	}

	public void StartClient()
	{
		if (NetworkManager.Singleton.StartClient())
		{
			Debug.Log("Client Started");

			// debugText = "Client Started";
		}
		else
		{
			Debug.Log("Client failed to Start");
			// debugText = "Client failed to Start";
		}
	}
}