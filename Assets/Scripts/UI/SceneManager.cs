using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

public class ManageScene : MonoBehaviour
{
    //Add scenes in inspector
    [SerializeField] private List<string> _sceneList;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene < _sceneList.Count)
        {
            string nextScene = _sceneList[currentScene +1];
            SceneManager.LoadScene(nextScene);
            // SceneManager.LoadScene(_sceneList[currentScene + 1].buildIndex);
        }
            
        else
            print("Its last scene");
    }
}
