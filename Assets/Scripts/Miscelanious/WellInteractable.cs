using UnityEngine;
using UnityEngine.UI;

public class WellInteractable : Interactable
{
    private bool startfading = false;
    [SerializeField] private float fadeToBlackSeconds;
    [SerializeField] private Image fadeToBlackPanel;
    private ManageScene manageScene;
    private float alphaColor = 0; private float increaseAlphaColor;
    private Color fadeColor;

    void Start()
    {
        manageScene = GameObject.Find("SceneManager").GetComponent<ManageScene>();
        increaseAlphaColor = fadeToBlackSeconds/1000;
    }

    public override void Interact()
    {
        startfading = true;
    }

    private void FixedUpdate()
    {
        if (startfading)
        {
            if (fadeToBlackSeconds > 0)
            {
                fadeToBlackSeconds -= 0.1f;
                alphaColor += increaseAlphaColor;
                fadeColor = new Color(0, 0, 0, alphaColor);
                fadeToBlackPanel.color = fadeColor;
            }
            if (fadeToBlackSeconds <= 0)
            {
                manageScene.LoadNextScene();
            }
        }
    }
}
