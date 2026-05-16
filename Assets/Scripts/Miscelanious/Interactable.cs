using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public GameObject _interactKey;

    public virtual void Interact()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _interactKey.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _interactKey.SetActive(false);
        }
    }
}
