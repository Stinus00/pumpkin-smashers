using UnityEngine;
using UnityEngine.UI;

public class EndGame : Interactable
{
    [SerializeField] private GameObject endGamePanel;

    public override void Interact()
    {
        GameObject menu = GameObject.Find("MenuManager");
        menu.GetComponent<ManageMenu>().PausePlayer(false);
        endGamePanel.SetActive(true);
    }
}

