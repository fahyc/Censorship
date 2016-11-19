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

    }

    public void requestGameList()
    {
        aws.RequestUpdate(this);
    }

    // actually host a game given the information
    public void hostGame()
    {
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