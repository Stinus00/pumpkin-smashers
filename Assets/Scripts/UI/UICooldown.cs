using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UICooldown : MonoBehaviour
{
    
    [SerializeField] private List<Image> cooldowns;
    private bool deactiveCooldown;

    private IPlayerActions playerActions;

    void Awake()
    {
        playerActions = FindObjectOfType<PlayerActions>();
    }

    void FixedUpdate()
    {
        if((playerActions.GetIsAttacking() || playerActions.GetIsDashing()) || playerActions.GetIsInAnimation())
        {
            deactiveCooldown = true;
            foreach(Image cooldown in cooldowns)
                cooldown.color = Color.grey;
        }
        else if (deactiveCooldown)
        {
            deactiveCooldown = false;
            foreach(Image cooldown in cooldowns)
                cooldown.color = Color.white;
        }
    }
}
