using UnityEngine;

public class BoomBox : Interactable
{
    private bool boomBoxOn;
    private AudioSource audioSource;

    [SerializeField] private Animator animator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        Debug.Log("Test");
        boomBoxOn = !boomBoxOn;
        if (boomBoxOn)
        {
            audioSource.Play();
            animator.SetTrigger("Start Loop");
        }
        if(!boomBoxOn)
        {
            audioSource.Stop();
            animator.SetTrigger("Stop Loop");

        }
    }
}
