using UnityEngine;

public class LandAudio : MonoBehaviour
{
    private IPlayerActions playerActions;
    private AudioSource audioSource;
    private bool jumping = false;

    void Awake()
    {
        playerActions = FindObjectOfType<PlayerActions>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if(playerActions.GetIsGrounded() && jumping)
        {
            audioSource.Play();
            jumping = false;
        }
            
        if(!playerActions.GetIsGrounded() && !jumping)
            jumping = true;
    }
}
