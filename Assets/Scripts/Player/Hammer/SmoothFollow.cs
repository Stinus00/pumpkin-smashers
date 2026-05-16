using UnityEngine;

public class SmoothFollowPlayer : MonoBehaviour 
{    
    public Transform leader;
    public float followSharpness = 0.1f;

    [SerializeField] private Vector3 _followOffset;

    private IPlayerActions playerActions;

    private bool stopMultiply = false;

    void Awake()
    {
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void LateUpdate () 
    {   
        Flip();

        // Apply that offset to get a target position.
        Vector3 targetPosition = leader.position + _followOffset;

        // Smooth follow.    
        transform.position += (targetPosition - transform.position) * followSharpness;
    }

    private void Flip()
    {
        if(playerActions.GetIsLookingRight() && !stopMultiply)
        {
            stopMultiply = true;
            _followOffset.x = _followOffset.x * -1;
            transform.Rotate(Vector3.up * 180);
        }
        else if(!playerActions.GetIsLookingRight() && stopMultiply)
        {
            stopMultiply = false;
            _followOffset.x = _followOffset.x * -1;
            transform.Rotate(Vector3.up * 180);
        }   
    }
}
