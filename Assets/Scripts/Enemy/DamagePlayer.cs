using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;

    [SerializeField] private bool boss = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(boss)
        {
            if(other.gameObject.tag == "PlayerHurtbox" && 
            GetComponent<MinibossEnemyAI>().GetCurrentState() != MinibossEnemyAI.State.Stunned && 
            GetComponent<MinibossEnemyAI>().GetCurrentState() != MinibossEnemyAI.State.Exhausted)
            {
                Debug.Log("Player hit");
                PlayerHurtBox player = other.gameObject.GetComponent<PlayerHurtBox>();
                player.PlayerHit(damage);
            }
        }
        else 
        {
            if(other.gameObject.tag == "PlayerHurtbox" && GetComponent<EnemyAI>().GetCurrentState() != EnemyAI.State.Stunned)
            {
                Debug.Log("Player hit");
                PlayerHurtBox player = other.gameObject.GetComponent<PlayerHurtBox>();
                player.PlayerHit(damage);
            }
        }
        
    }
}
