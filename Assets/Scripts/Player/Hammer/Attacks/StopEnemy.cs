using UnityEngine;

public class StopEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRB2D;
    private bool stopPlayer = false;

    void OnEnable()
    {
        stopPlayer = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            stopPlayer = true;
        }
    }
    
    void FixedUpdate()
    {
        if(stopPlayer)
            playerRB2D.linearVelocity = Vector2.zero;
    }
}
