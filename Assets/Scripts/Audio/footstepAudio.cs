using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    private IPlayerActions playerActions;
    private AudioSource audioSource;

    void Awake()
    {
        playerActions = FindObjectOfType<PlayerActions>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if(playerActions.GetIsWalking() && !audioSource.isPlaying && playerActions.GetIsGrounded())
            audioSource.Play();
        if((!playerActions.GetIsWalking() || !playerActions.GetIsGrounded()) && audioSource.isPlaying)
            audioSource.Stop();
    }
}
