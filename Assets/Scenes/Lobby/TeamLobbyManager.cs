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

    public static List<int> playerIdeas = new List<int>();

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

    public void updateClientNumbers()
    {
        foreach(TeamLobbyPlayer p in lobbySlots)
        {
            if (p != null)
                p.RpcUpdateClientNumbers(playerCount);
        }
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        PlayerStartPosition p = GetStartPosition().GetComponent<PlayerStartPosition>();
        GameObject go = (GameObject)GameObject.Instantiate(gamePlayerPrefab, p.transform.position, Quaternion.identity);
        go.GetComponent<Global>().playerIdeaIndex = p.ideaIndex;
        playerIdeas.Add(p.ideaIndex);
        go.SetActive(true);
        return go;
    }
}
