using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyMenu : MonoBehaviour, IPListener {

    TeamLobbyManager manager;
    public GameObject mainPanel;

    // prefabs
    public GameObject lobbyManagerPrefab;
    public GameObject scrollViewPrefab;
    public GameObject gameInfoPrefab;
    public GameObject hostMenuPrefab;
    public GameObject lobbyInfoPrefab;

    public static LobbyMenu _instance;

    Text playerCountText;

    GameObject scrollView;
    GameObject gameList;
    GameObject gameHostMenu;
    GameObject lobbyInfo;

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
        public int port;
        public int Time;
        public string name;
        public string scenario;
        public int maxPlayers;
        public bool isInternetGame;
    }

    List<GameInfo> availableGames;
    // GameInfo hostingGame;

    void Awake()
    {
        _instance = this;
        Initialize();
    }
    
    public void Start()
    {
        scrollView = Instantiate(scrollViewPrefab);
        scrollView.transform.SetParent(mainPanel.transform);
        gameList = scrollView.transform.Find("Viewport/Content").gameObject;
    }

    public void Initialize()
    {
        if (manager == null)
        {
            manager = GameObject.FindObjectOfType<TeamLobbyManager>();
            if (manager == null)
                manager = Instantiate(lobbyManagerPrefab).GetComponent<TeamLobbyManager>();
        }
        Debug.Log("Initializing lobby menu");
        aws = manager.GetComponent<AWSTest>();
    }

    public void getData(string data)
    {
        string cleanData = data.Replace(@"\", string.Empty);
        cleanData = cleanData.Substring(1, cleanData.Length - 2);

        GameInfoCollection info = JsonUtility.FromJson<GameInfoCollection>(cleanData);

        refreshGameList(info.arr);
    }

    public void clearList()
    {
        lobbyInfo = null;
        if (gameList != null)
        {
            foreach (Transform i in gameList.transform)
            {
                GameObject.Destroy(i.gameObject);
            }
        }
    }

    void refreshGameList(GameInfo[] info)
    {
        if (gameList == null)
            return;

        clearList();

        foreach (GameInfo i in info)
        {
            addGame(i);
        }
    }

    void addGame(GameInfo info)
    {
        GameObject newGame = Instantiate(gameInfoPrefab);
        newGame.transform.Find("GameName").GetComponent<Text>().text = info.name;
        newGame.transform.Find("ScenarioName").GetComponent<Text>().text = info.scenario;
        newGame.transform.Find("NumPlayers").GetComponent<Text>().text = newGame.transform.Find("NumPlayers").GetComponent<Text>().text.Replace("[max]", info.maxPlayers.ToString());
        newGame.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => joinGame(info));
        if (info.isInternetGame)
            newGame.transform.Find("IsLocal").GetComponent<Text>().text = "Internet";
        else
            newGame.transform.Find("IsLocal").GetComponent<Text>().text = "Local";

        newGame.transform.SetParent(gameList.transform);
    }

    void joinGame(GameInfo info)
    {
        displayLobbyInfo(info);
        lobbyInfo.transform.Find("Buttons/CancelButton").GetComponent<Button>().onClick.AddListener(requestGameList);
        lobbyInfo.transform.Find("Buttons/CancelButton").GetComponent<Button>().onClick.AddListener(manager.StopClient);

        // init UPnP for client as well
        Debug.Log("Attempting to join game at " + info.ip + ":" + info.port);

        // assign proper IP first
        manager.networkAddress = info.ip;
        manager.networkPort = info.port;
        manager.StartClient();
    }

    // Show a screen to set up the information for the game being hosted 
    public void toggleHostScreen()
    {
        // stop hosting an existing game / close client
        clearList();
        if (gameHostMenu == null)
        {
            // Display hosting menu
            gameHostMenu = Instantiate(hostMenuPrefab);
            gameHostMenu.transform.SetParent(gameList.transform);

            gameHostMenu.transform.Find("Buttons/HostButton").GetComponent<Button>().onClick.AddListener(hostGame);
            gameHostMenu.transform.Find("Buttons/CancelButton").GetComponent<Button>().onClick.AddListener(toggleHostScreen);
            gameHostMenu.transform.Find("Buttons/CancelButton").GetComponent<Button>().onClick.AddListener(requestGameList);

            // also disable refreshing game list
            mainPanel.transform.Find("ButtonPanel/RefreshButton").GetComponent<Button>().interactable = false;
            mainPanel.transform.Find("ButtonPanel/HostGameButton").GetComponent<Button>().interactable = false;
        }
        else {
            gameHostMenu = null;

            // also enable refreshing game list
            mainPanel.transform.Find("ButtonPanel/HostGameButton").GetComponent<Button>().interactable = true;
            mainPanel.transform.Find("ButtonPanel/RefreshButton").GetComponent<Button>().interactable = true;
        }
    }

    public void requestGameList()
    {
        // first stop client/host if needed
        manager.StopHost();
        Debug.Log("requesting game list");
        // make refresh button usable again
        mainPanel.transform.Find("ButtonPanel/RefreshButton").GetComponent<Button>().interactable = true;
        mainPanel.transform.Find("ButtonPanel/HostGameButton").GetComponent<Button>().interactable = true;

        aws.RequestUpdate(this);
    }

    public void exitLobbyMenu(int index)
    {
        // cleanly exit lobby stuff
        manager.StopHost();
        manager.dontDestroyOnLoad = false;
        Debug.Log("Destroying manager!");
        Destroy(manager.gameObject);
        TeamLobbyManager.Shutdown();
        SceneManager.LoadScene(index);
    }

    // actually host a game given the information
    public void hostGame()
    {
        // First get values for game to host
        GameObject scenarioDropdown = gameHostMenu.transform.Find("Scenario").gameObject;
        GameObject playerDropdown = gameHostMenu.transform.Find("MaxPlayers").gameObject;

        string gameName = gameList.GetComponentInChildren<InputField>().text;
        string scenarioName = scenarioDropdown.GetComponentInChildren<Dropdown>().GetComponentInChildren<Text>().text;
        int maxPlayers = playerDropdown.GetComponentInChildren<Dropdown>().value + 1;   // add 1 since index zero = 1 player

        // prevent empty name
        if (gameName == "")
            gameName = "Xenonet Game";

        // set up UPnP if possible
        manager.InitializePorts();
        if (TeamLobbyManager.upnp_enabled)
        {
            manager.networkAddress = TeamLobbyManager.my_ip;
            manager.networkPort = TeamLobbyManager.public_port;
        }

        GameInfo info = new GameInfo();
        info.ip = TeamLobbyManager.my_ip;
        info.port = TeamLobbyManager.public_port;
        info.name = gameName;
        info.scenario = scenarioName;
        info.maxPlayers = maxPlayers;
        info.isInternetGame = TeamLobbyManager.upnp_enabled;

        // show the proper info onscreen
        displayLobbyInfo(info);

        // let the player know if UPnP won't work
        lobbyInfo.transform.Find("LocalIPWarning").gameObject.SetActive(!TeamLobbyManager.upnp_enabled);

        lobbyInfo.transform.Find("Buttons/CancelButton").GetComponent<Button>().onClick.AddListener(manager.StopHost);
        // TODO: need to notify web server about removal of game

        // initialize the lobby first
        manager.initializeLobby(scenarioName, maxPlayers);
        // then start hosting
        manager.StartHost();

        Coroutine updateRoutine = StartCoroutine(resendGameInfo(info));
        lobbyInfo.transform.Find("Buttons/CancelButton").GetComponent<Button>().onClick.AddListener(() => StopCoroutine(updateRoutine));
    }

    IEnumerator resendGameInfo(GameInfo info)
    {
        while (true)
        {
            // Send the update every 15 seconds to keep the game listed online
            if (aws)
                aws.SendUpdate(JsonUtility.ToJson(info, true));
            yield return new WaitForSeconds(15.0f);
        }
    }

    public void updatePlayerCount(int n)
    {
        if (playerCountText)
            playerCountText.text = n.ToString();
    }

    void displayLobbyInfo(GameInfo info)
    {
        clearList();

        if (gameHostMenu)
            gameHostMenu = null;

        // show game info
        lobbyInfo = Instantiate(lobbyInfoPrefab);
        lobbyInfo.transform.SetParent(gameList.transform);

        // display proper text for everything
        lobbyInfo.transform.Find("GameName").GetComponent<Text>().text = lobbyInfo.transform.Find("GameName").GetComponent<Text>().text.Replace("[name]", info.name);
        lobbyInfo.transform.Find("Scenario").GetComponent<Text>().text = lobbyInfo.transform.Find("Scenario").GetComponent<Text>().text.Replace("[name]", info.scenario);
        lobbyInfo.transform.Find("PlayerCount/Current").GetComponent<Text>().text = "0";
        lobbyInfo.transform.Find("PlayerCount/Max").GetComponent<Text>().text = info.maxPlayers.ToString();

        Button ready = lobbyInfo.transform.Find("Buttons/ReadyButton").GetComponent<Button>();
        ready.onClick.AddListener(() => ready.interactable = false);

        mainPanel.transform.Find("ButtonPanel/RefreshButton").GetComponent<Button>().interactable = false;
        mainPanel.transform.Find("ButtonPanel/HostGameButton").GetComponent<Button>().interactable = false;

        Text t = lobbyInfo.transform.Find("PlayerCount/Current").GetComponent<Text>();
        playerCountText = t;
    }

    public GameObject getLobbyInfo()
    {
        return lobbyInfo;
    }
}