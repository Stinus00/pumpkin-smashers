using UnityEngine;

public interface IInteractable
{
    void Interact();

    void OnTriggerEnter2D(Collider2D other);
    void OnTriggerExit2D(Collider2D other);

}
