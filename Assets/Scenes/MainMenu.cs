using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour
{

    public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void loadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void exitApp()
    {
        Application.Quit();
    }
}
