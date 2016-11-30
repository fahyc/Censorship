using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class TeamLobbyPlayer : NetworkLobbyPlayer {

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        TeamLobbyManager._singleton.updatePlayerCount(1);
        LobbyMenu._instance.updatePlayerCount(TeamLobbyManager._singleton.playerCount);
    }

    public override void OnStartLocalPlayer()
    {
        // bind the ready button for the player
        GameObject lobbyInfo = LobbyMenu._instance.getLobbyInfo();

        if (lobbyInfo)
        {
            if (isLocalPlayer)
            {
                lobbyInfo.transform.Find("Buttons/ReadyButton").GetComponent<Button>().onClick.AddListener(SendReadyToBeginMessage);
            }
            else
                Debug.LogWarning("Not local player! not binding buttons");
        }
        else
            Debug.LogWarning("Lobby info panel not found");

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
