using UnityEngine;
using System.Collections;

public class OnTriggerHitbox : MonoBehaviour
{
    [Header("General Stats")]
    public float damage = 10f;
    public float knockback = 50f;
    public float freezeTime = 0.05f;

    [Header("Miscellanious")]
    public bool up = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("BossEnemy"))
        {
            Debug.Log("Enemy Hit!");
            // StartCoroutine(FreezeFrame());

            // Remove health from enemy
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            enemyStats.Damage(damage);
            
            // Apply Knockback
            if(other.gameObject.CompareTag("Enemy"))
            {
                Rigidbody2D rb2D = other.GetComponent<Rigidbody2D>();

                Vector3 direction;
                if(up)
                {
                    direction = Vector2.up;
                }
                else
                {
                    direction = transform.right;
                }
                rb2D.linearVelocity = Vector2.zero;
                rb2D.AddForce(direction * knockback, ForceMode2D.Impulse);
            }

            // Stun Counter
            if(other.gameObject.CompareTag("BossEnemy") && gameObject.CompareTag("LightAttackHitbox"))
            {
                other.gameObject.GetComponent<MinibossEnemyAI>().AddToCounter();
            }
        }
    }

    // // Add Freeze Frame to attack.
    // IEnumerator FreezeFrame()
    // {
    //     Time.timeScale = 0f;
    //     Debug.Log("FREEZE");
    //     yield return new WaitForSecondsRealtime(freezeTime);
    //     Debug.Log("UnFreeze");
    //     Time.timeScale = 1f;
    // }
}
