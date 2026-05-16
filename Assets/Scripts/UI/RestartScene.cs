using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{

    public void RestartCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);
    }

    public void BackToMainMenu()
    {
        GameObject menu = GameObject.Find("MenuManager");
        menu.GetComponent<ManageMenu>().PausePlayer(true);   
        SceneManager.LoadScene("Menu");
    }
}