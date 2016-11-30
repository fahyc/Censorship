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

    public static IPAddress my_ip;

    void Start()
    {
        try
        {
            InitUPnP().Wait();
        }
        catch (AggregateException ae)
        {
            // port forwarding failed or timed out

        }

        Debug.Log("External ip after port mapping: " + my_ip.ToString());

        _singleton = this;
        playerCount = 0;
    }

    private static Task InitUPnP()
    {
        // Set up the DeviceFound and DeviceLost methods
        NatDiscoverer disc = new NatDiscoverer();
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(5000);  // 5 second timeout for device discovery

        NatDevice device = null;

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
                my_ip = task.Result;
                Task res = null;
                int pubPort = 7777;
                while (pubPort < 8000)
                {
                    try
                    {
                        res = device.CreatePortMapAsync(new Mapping(Protocol.Tcp, 7777, pubPort, "Xenonet Server (TCP)"));
                        break;
                    }
                    catch (MappingException me)
                    {
                        pubPort++;
                    }
                }
                return res;
            })
            .Unwrap()
            .ContinueWith(task =>
            {
                Task res = null;
                int pubPort = 7777;
                while (pubPort < 8000)
                {
                    try
                    {
                        res = device.CreatePortMapAsync(new Mapping(Protocol.Udp, 7777, pubPort, "Xenonet Server (UDP)"));
                        break;
                    }
                    catch (MappingException me)
                    {
                        pubPort++;
                    }
                }
                return res;
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

        int i = Random.Range(0, playerIdeas.Count);

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
