using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ManageMenu : MonoBehaviour
{
    private bool _gameStarted = false;
    private bool _gamePaused = false;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _moveMenu;

    [SerializeField] private GameObject _playerUI;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private bool _mainMenu = false;

    void Awake()
    {
        if(_mainMenu == false)
            DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.Find("Player");
        _playerInput = player.GetComponent<PlayerInput>();
        _playerUI = GameObject.Find("PlayerUI");
    }

    public void OnPause()
    {
        if(_gameStarted)
        {
            if(!_gamePaused)
            {
                _pauseMenu.SetActive(true);
                _gamePaused = true;
                PausePlayer(false);
            }
            else
            {
                _pauseMenu.SetActive(false);
                _gamePaused = false;
                PausePlayer(true);
            }
                
        }
    }

    public void PausePlayer(bool value)
    {
        _playerInput.enabled = value;
        _playerUI.SetActive(value);
        if(!value)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

    }

    public void StartGame()
    {
        _gameStarted = true;
    }

    public void MoveMenu()
    {
        _moveMenu.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    public void Back()
    {
        _moveMenu.SetActive(false);
        _pauseMenu.SetActive(true);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
