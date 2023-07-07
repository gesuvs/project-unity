using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{

	[SerializeField] private string environment = "dev";

	[SerializeField] private int maxConnections = 4;

	public bool IsRelayEnabled =>
		Transport != null && 
		Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;


	public async Task<RelayHostData> SetupRelay()
	{
		
		Debug.Log($"Relay Server Starting with max connections: {maxConnections}");
		
		
		var options = new InitializationOptions()
			.SetEnvironmentName(environment);

		await UnityServices.InitializeAsync(options);

		if (!AuthenticationService.Instance.IsSignedIn)
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}

		var allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);

		var relayHostData = new RelayHostData
		{
			Key = allocation.Key,
			Port = (ushort)allocation.RelayServer.Port,
			AllocationID = allocation.AllocationId,
			AllocationIDBytes = allocation.AllocationIdBytes,
			IPv4Address = allocation.RelayServer.IpV4,
			ConnectionData = allocation.ConnectionData
		};

		relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

		Transport.SetRelayServerData(
			relayHostData.IPv4Address,
			relayHostData.Port,
			relayHostData.AllocationIDBytes,
			relayHostData.Key,
			relayHostData.ConnectionData);
		
		Debug.Log($"Relay Server generated a join code: {relayHostData.JoinCode}");

		return relayHostData;

	}

	public UnityTransport Transport => NetworkManager
		.Singleton
		.gameObject
		.GetComponent<UnityTransport>();

}