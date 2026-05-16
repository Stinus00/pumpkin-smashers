using UnityEngine;

public class WallChecker : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerDashing dashing;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("CameraWall"))
        {
            controller.SetIsWalled(true);
            dashing.StopDashImmediate();
        }   
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("CameraWall"))
        {
            controller.SetIsWalled(false);
        }   
    }
}
