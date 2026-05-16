using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collectable : MonoBehaviour
{

    [SerializeField] private float timeDeactive = 10.0f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(Activate(timeDeactive));
        }
        if(collision.gameObject.CompareTag("PlayerHurtbox"))
        {
            PlayerHurtBox player = collision.gameObject.GetComponent<PlayerHurtBox>();
            player.PlayerHit(10000);
        }
    }

    IEnumerator Activate(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
