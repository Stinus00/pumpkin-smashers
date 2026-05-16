using UnityEngine;

public class ShakeScreen : MonoBehaviour
{

    [Header("Screen Shake")]
    private Transform transform;
    [SerializeField] private float shakeDuration = 0f;
    [SerializeField] private float shakeMagnitude = 0.7f;
    [SerializeField] private float dampingSpeed = 1.0f;
    public bool shaking;

    void Awake()
    {
        // Set transform
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void Update()
    {
        // Check if shakeDuration is available and start shaking screen
        if (shakeDuration > 0)
        {
            transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeMagnitude;
            
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if(shaking)
        {
            shaking = false;
        }
    }

    public void TriggerShake(float duration = 2.0f) 
    {
        shakeDuration = duration;
        shaking = true;
    }
}
