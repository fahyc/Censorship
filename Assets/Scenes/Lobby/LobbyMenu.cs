using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyMenu : MonoBehaviour, IPListener {

    public TeamLobbyManager manager;
    public GameObject mainPanel;

    // prefabs
    public GameObject scrollViewPrefab;
    public GameObject gameInfoPrefab;
    public GameObject hostMenuPrefab;

    NetworkManagerHUD managerHUD;

    GameObject scrollView;
    GameObject gameList;

    AWSTest aws;

    [System.Serializable]
    public class GameInfoCollection
    {
        public GameInfo[] arr;
    }

    [System.Serializable]
    public class GameInfo
    {
        public string ip;
        public int Time;
        public string name;
    }

    List<GameInfo> availableGames;
    // GameInfo hostingGame;

    void Start()
    {
        managerHUD = manager.GetComponent<NetworkManagerHUD>();
        aws = manager.GetComponent<AWSTest>();
        scrollView = Instantiate(scrollViewPrefab);
        scrollView.transform.SetParent(mainPanel.transform);
        gameList = scrollView.transform.Find("Viewport/Content").gameObject;
    }

    public void getData(string data)
    {
        string cleanData = data.Replace(@"\", string.Empty);
        cleanData = cleanData.Substring(1, cleanData.Length - 2);

        GameInfoCollection info = JsonUtility.FromJson<GameInfoCollection>(cleanData);

        refreshGameList(info.arr);
    }

    void refreshGameList(GameInfo[] info)
    {
        availableGames = new List<GameInfo>(info);

        foreach (Transform i in gameList.transform)
        {
            GameObject.Destroy(i.gameObject);
        }

        foreach (GameInfo i in availableGames)
        {
            addGame(i);
        }
    }

    void addGame(GameInfo info)
    {
        GameObject newGame = Instantiate(gameInfoPrefab);
        newGame.transform.Find("GameName").GetComponent<Text>().text = info.name;
        newGame.transform.SetParent(gameList.transform);
    }


    // Show a screen to set up the information for the game being hosted 
    public void toggleHostScreen()
    {
        // clear out game list
        foreach (Transform i in gameList.transform)
        {
            GameObject.Destroy(i.gameObject);
        }

        // Display hosting menu
        GameObject hostMenu = Instantiate(hostMenuPrefab);
        hostMenu.transform.SetParent(gameList.transform);
    }

    public void requestGameList()
    {
        aws.RequestUpdate(this);
    }

    // actually host a game given the information
    public void hostGame()
    {
        // First get values for game to host
        GameObject scenarioDropdown = gameList.transform.Find("Scenario").gameObject;
        GameObject playerDropdown = gameList.transform.Find("Scenario").gameObject;

        string gameName = gameList.GetComponentInChildren<InputField>().GetComponentInChildren<Text>().text;
        string scenarioName = scenarioDropdown.GetComponentInChildren<Dropdown>().GetComponentInChildren<Text>().text;
        int maxPlayers = playerDropdown.GetComponentInChildren<Dropdown>().value + 2;   // add 2 since index zero = 2 players

        // Clear out old host button
        foreach (Transform i in gameList.transform)
        {
            GameObject.Destroy(i.gameObject);
        }

        // show new info
        GameObject gameInfo = Instantiate(gameInfoPrefab);
        gameInfo.transform.SetParent(gameList.transform);

        gameInfo.transform.Find("GameName").GetComponent<Text>().text = gameInfo.transform.Find("GameName").GetComponent<Text>().text.Replace("[name]", gameName);
        gameInfo.transform.Find("Scenario").GetComponent<Text>().text = gameInfo.transform.Find("Scenario").GetComponent<Text>().text.Replace("[name]", scenarioName);
        gameInfo.transform.Find("PlayerCount").GetComponent<Text>().text = gameInfo.transform.Find("PlayerCount").GetComponent<Text>().text.Replace("[cur]", "0").Replace("[max]", maxPlayers.ToString());


        // Now actually do the work of hosting the game

        GameInfo info = new GameInfo();
        info.ip = Network.player.ipAddress;
        info.name = "New game";
        aws.SendUpdate(JsonUtility.ToJson(info, true));
    }

    // for debug purposes
    public void toggleNetManagerGUI()
    {
        manager.showLobbyGUI = !manager.showLobbyGUI;
        managerHUD.showGUI = !managerHUD.showGUI;
    }
}