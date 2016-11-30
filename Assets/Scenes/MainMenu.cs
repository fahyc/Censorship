using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour
{

    public void loadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }

    public void loadLevel(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public void exitApp()
    {
        Application.Quit();
    }
}
