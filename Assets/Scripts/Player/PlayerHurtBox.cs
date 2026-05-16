using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHurtBox : MonoBehaviour
{
    public float totalhealth = 100.0f;
    private float remainingHealth = 100.0f;
    public float invincibilityFrames = 0.0f;

    public GameObject retryScreen;
    public RectTransform healthBarUI;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float spriteDamagedTimer = 0.2f;
    private Material material;

    private float healthBarUIWidth;
    
    void Start()
    {
        material = spriteRenderer.material;
        healthBarUIWidth = healthBarUI.sizeDelta.x;
    }

    public void PlayerHit(float damage)
    {
        if(invincibilityFrames <= 0)
        {
            remainingHealth = remainingHealth - damage;
            StartCoroutine("DamageSprite");
        }
        if(remainingHealth <= 0)
        {
            Debug.Log("Player dead lol noob git gud");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            retryScreen.SetActive(true);
            Destroy(player);
        }
    }

    public void AddInvincibility(float f_invincibility)
    {
        invincibilityFrames = f_invincibility;
    }

    void FixedUpdate()
    {
        if(invincibilityFrames > 0)
            invincibilityFrames = invincibilityFrames - 0.01f;
    }

    private IEnumerator DamageSprite()
    {
        GetComponent<AudioSource>().Play();

        float healthPercentage = remainingHealth/totalhealth;
        Color blendColor = LerpColor(healthPercentage);
        SetHealthUI(healthPercentage);
        Debug.Log(blendColor);

        material.SetFloat("_FlashAmount", 0.3f);
        material.SetColor("_FlashColor", blendColor);
        yield return new WaitForSeconds(spriteDamagedTimer);
        material.SetFloat("_FlashAmount", 0f);
    }

    private Color LerpColor(float healthPercentage)
    {
        if (healthPercentage <= 0.5f)
        {
            return Color.Lerp (Color.red, Color.yellow, (healthPercentage-.1f)*2);
        }
        else
        {
            return Color.Lerp (Color.yellow, Color.white, (healthPercentage-.5f)*2);
        }
    }

    private void SetHealthUI(float healthPercentage)
    {
        healthBarUI.sizeDelta = new Vector2(healthBarUIWidth * healthPercentage, healthBarUI.sizeDelta.y);
    }
}
