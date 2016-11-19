using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour {

    public TeamLobbyManager manager;
    public GameObject mainPanel;

    // prefabs
    public GameObject scrollViewPrefab;
    public GameObject gameInfoPrefab;

    NetworkManagerHUD managerHUD;

    GameObject scrollView;
    GameObject gameList;

    // ArrayList<CustomClass> availableGames;
    // CustomClass hostingGame;

    void Start()
    {
        managerHUD = manager.GetComponent<NetworkManagerHUD>();
        scrollView = Instantiate(scrollViewPrefab);
        scrollView.transform.SetParent(mainPanel.transform);
        gameList = scrollView.transform.Find("Viewport/Content").gameObject;
    }

    public void addGame(string name)
    {
        GameObject newGame = Instantiate(gameInfoPrefab);
        newGame.transform.Find("GameName").GetComponent<Text>().text = name;
        newGame.transform.SetParent(gameList.transform);
    }

    // Show a screen to set up the information for the game being hosted 
    public void toggleHostScreen()
    {

    }

    // actually host a game given the information
    public void hostGame()
    {

    }

    // for debug purposes
    public void toggleNetManagerGUI()
    {
        manager.showLobbyGUI = !manager.showLobbyGUI;
        managerHUD.showGUI = !managerHUD.showGUI;
    }
}
