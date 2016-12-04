using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Open.Nat;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System;

public class TeamLobbyManager : NetworkLobbyManager {

    public List<int> ideas;
    public static List<int> playerIdeas;
    List<int> ideasLeft;

    public static TeamLobbyManager _singleton;

    Dictionary<NetworkConnection, int> teamAssignments;

    Dictionary<int, PlayerStartPosition> initialPositions;

    public int playerCount;

    public static string my_ip;
    public static int public_port = 7777;
    public static bool upnp_enabled;

    void Start()
    {
        _singleton = this;
        playerCount = 0;
    }

    public void InitializePorts()
    {
        upnp_enabled = true;
        try
        {
            InitUPnP().Wait();
        }
        catch (AggregateException ae)
        {
            // port forwarding failed or timed out
            upnp_enabled = false;
            my_ip = Network.player.ipAddress;
        }

        Debug.Log("External ip after port mapping: " + my_ip);
    }

    private static Task InitUPnP()
    {
        // Set up the DeviceFound and DeviceLost methods
        NatDiscoverer disc = new NatDiscoverer();
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(3000);  // 3 second timeout for device discovery

        NatDevice device = null;
        public_port = 7777; // default port

        return disc.DiscoverDeviceAsync(PortMapper.Upnp, cts)
            .ContinueWith(task =>
            {
                device = task.Result;
                Debug.Log("device found: " + device.ToString());
                return device.GetExternalIPAsync();
            })
            .Unwrap()
            .ContinueWith(task =>
            {
                my_ip = task.Result.ToString();
                while (public_port < 8000)
                {
                    try
                    {
                        device.CreatePortMapAsync(new Mapping(Protocol.Tcp, 7777, public_port, "Xenonet Server (TCP)"));
                        device.CreatePortMapAsync(new Mapping(Protocol.Udp, 7777, public_port, "Xenonet Server (UDP)"));
                        break;
                    }
                    catch (MappingException me)
                    {
                        public_port++;
                    }
                }
                return device.GetAllMappingsAsync();
            })
            .Unwrap()
            .ContinueWith(task =>
            {
                foreach (Mapping m in task.Result)
                {
                    Debug.Log("Mapping found: " + (m.Protocol == Protocol.Tcp ? "TCP: " : "UDP:" ) + my_ip + ":" + m.PublicPort + " -> " + m.PrivateIP + ":" + m.PrivatePort + "; " + m.Description);
                }
            });
    }

    public void initializeLobby(string scenario, int players)
    {
        maxPlayers = players;
        // TODO: change playScene based on scenario

        playScene = scenario;
    }

    public void updatePlayerCount(int n)
    {
        playerCount += n;
    }

    public override void OnLobbyStartServer()
    {
        if (ideas.Count < maxPlayers)
            Debug.LogWarning("Possible to have players on same team! maxPlayers > ideaCount");

        teamAssignments = new Dictionary<NetworkConnection, int>();
        playerIdeas = new List<int>(ideas);
    }
	
	public override GameObject OnLobbyServerCreateLobbyPlayer (NetworkConnection conn, short playerControllerId) {

        if (playerIdeas.Count <= 0)
        {
            Debug.LogWarning("Run out of team assignments! Defaulting to 0");
            teamAssignments.Add(conn, 0);
        }

        int i = UnityEngine.Random.Range(0, playerIdeas.Count);

        teamAssignments.Add(conn, playerIdeas[i]);

        playerIdeas.RemoveAt(i);

        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
	}

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        foreach (PlayerStartPosition p in FindObjectsOfType<PlayerStartPosition>())
        {
            if (p.ideaIndex == teamAssignments[conn])
            {
                GameObject go = (GameObject)GameObject.Instantiate(gamePlayerPrefab, p.transform.position, Quaternion.identity);
                go.GetComponent<Global>().playerIdeaIndex = teamAssignments[conn];

                go.SetActive(true);

                return go;
            }
        }

        Debug.LogWarning("Unable to find start position with correct idea index! player spawn is null");
        return null;
    }


    // Client callbacks
    // ===========

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
    }
}
