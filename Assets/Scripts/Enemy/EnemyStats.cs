using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour
{
    [Header("Statistics")]
    [SerializeField] private float totalhealth = 100.0f;
    [SerializeField] private bool box = false;
    private float remainingHealth;
    private Rigidbody2D rb;
    private Collider2D col;
    private ParticleSystem particles;
    private SpriteRenderer spriteRenderer;
    private Material material;
    private EnemyCounting arena;
    private bool alreadyDying = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public List<AudioClip> hurtSounds;

    [SerializeField] private float spriteDamagedTimer = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        col = GetComponent<Collider2D>();

        remainingHealth = totalhealth;

        material = spriteRenderer.material;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Kill Enemy if no health remaining.
        if(remainingHealth <= 0 && !alreadyDying)
        {
            alreadyDying = true;
            StartCoroutine("KillEnemy");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Arena"))
        {
            arena = other.gameObject.GetComponent<EnemyCounting>();
        }
    }

    IEnumerator KillEnemy()
    {
        // Disable collider and spriterender and wait for the particles to finish before destroying the enemy.
            col.enabled = false;
            spriteRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(particles.main.startLifetime.constantMax);
            // Destroy Enemy GameObject and remove from arena counter if applicable.
            if(arena)
                arena.DestroyEnemy(gameObject);
            Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        remainingHealth -= damage;
        StartCoroutine("DamageSprite");
    }

    private IEnumerator DamageSprite()
    {
        if(!box)
        {
            particles.Play();
            int randomNumber = Random.Range(0, hurtSounds.Count-1);
            audioSource.clip = hurtSounds[randomNumber];
        }
        audioSource.Play();
        
        float healthPercentage = remainingHealth/totalhealth;
        Color blendColor = LerpColor(healthPercentage);
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
}
