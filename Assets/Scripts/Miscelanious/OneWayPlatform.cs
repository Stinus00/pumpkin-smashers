using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "PlatformChecker")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "PlatformChecker")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
