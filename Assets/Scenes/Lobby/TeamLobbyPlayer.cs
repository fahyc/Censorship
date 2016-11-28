using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TeamLobbyPlayer : NetworkLobbyPlayer {

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        TeamLobbyManager._singleton.updatePlayerCount(1);
        LobbyMenu._instance.updatePlayerCount(TeamLobbyManager._singleton.playerCount);
    }

    public void OnDestroy()
    {
        TeamLobbyManager._singleton.updatePlayerCount(-1);

        if (isLocalPlayer)
        {
            LobbyMenu._instance.clearList();
        }
        else {
            LobbyMenu._instance.updatePlayerCount(TeamLobbyManager._singleton.playerCount);
        }
    }
}
