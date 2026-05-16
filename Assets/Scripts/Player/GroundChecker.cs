using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private Transform parent;
    public float coyoteTime = 0.1f;

    private IPlayerActions playerActions;

    void Awake()
    {
        parent = this.transform.parent;
    }

    void Start()
    {
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("test");
            playerActions.SetIsGrounded(true);
            playerActions.SetIsJumping(false);
            playerActions.SetIsFalling(false);
        }   
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Invoke(nameof(CoyoteTime), coyoteTime);
        }   
    }

    void CoyoteTime()
    {
        playerActions.SetIsGrounded(false);
    }
}
