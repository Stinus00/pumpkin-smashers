using UnityEngine;

public class DestroyOnBossKill : MonoBehaviour
{
    [SerializeField] GameObject boss;
    
    void Update()
    {
        if(!boss)
            Destroy(gameObject);
    }
}
