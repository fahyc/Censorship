using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class TeamLobbyPlayer : NetworkLobbyPlayer {

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        // initial count update
        TeamLobbyManager._singleton.playerCount++;
        LobbyMenu._instance.updatePlayerCount(TeamLobbyManager._singleton.playerCount);
        TeamLobbyManager._singleton.updateClientNumbers();
    }

    public void updateClientNumbers(int count)
    {
        LobbyMenu._instance.updatePlayerCount(count);
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
        TeamLobbyManager._singleton.playerCount--;
        TeamLobbyManager._singleton.updateClientNumbers();

        if (isLocalPlayer)
        {
            LobbyMenu._instance.clearList();
            LobbyMenu._instance.requestGameList();
        }
    }
}
