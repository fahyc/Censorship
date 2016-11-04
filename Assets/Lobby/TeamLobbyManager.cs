using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TeamLobbyManager : NetworkLobbyManager {

    public int ideaCount;

    Dictionary<NetworkConnection, int> teamAssignments;
    List<int> assignmentsLeft;

    void Start()
    {
        if (ideaCount < maxPlayers)
            Debug.LogWarning("Possible to have players on same team! maxPlayers > ideaCount");

        teamAssignments = new Dictionary<NetworkConnection, int>();
        assignmentsLeft = new List<int>();

        for (int i=0; i<ideaCount; i++)
        {
            assignmentsLeft.Add(i);
        }
    }
	
	public override GameObject OnLobbyServerCreateLobbyPlayer (NetworkConnection conn, short playerControllerId) {

        if (assignmentsLeft.Count <= 0)
        {
            Debug.LogWarning("Run out of team assignments! Defaulting to 0");
            teamAssignments.Add(conn, 0);
        }

        int i = Random.Range(0, assignmentsLeft.Count);

        teamAssignments.Add(conn, assignmentsLeft[i]);

        assignmentsLeft.RemoveAt(i);

        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
	}

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject go = (GameObject)GameObject.Instantiate(gamePlayerPrefab, startPositions[conn.connectionId].position, Quaternion.identity);
        go.GetComponent<Global>().playerIdeaIndex = teamAssignments[conn];

        go.SetActive(true);

        return go;
    }
}
