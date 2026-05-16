using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public Transform Player;
    public Vector3 Offset;

    void Update()
    {
        if(!GetComponent<ShakeScreen>().shaking)
            transform.position = new Vector3(Player.position.x, Offset.y, Offset.z);
    }
}
