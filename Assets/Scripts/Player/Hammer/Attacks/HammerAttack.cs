using UnityEngine;
using System.Collections;

public class HammerAttack : MonoBehaviour
{
    public GameObject attack;

    [Header("Timing")]
    public float startAttack = 0.0f;
    public float stopAttack = 0.2f;

    [Header("Animators (Hammer Sprite)")]
    [SerializeField] private string animationString;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform animationPosition;
    [SerializeField] private SmoothFollowPlayer animationFollow;
    [SerializeField] private Vector3 animationOffset;

    private bool offsetAnim = false;

    public IPlayerActions playerActions;

    void OnEnable()
    {
        playerActions = FindObjectOfType<PlayerActions>();
        StartCoroutine("ActivateHitboxAttack");
        StartCoroutine("DeactivateHitboxAttack");
    }

    void Update()
    {
        if(offsetAnim)
            OffsetAnimation();
    }

    void OnDisable()
    {
        playerActions.SetIsAttacking(false);
    }

    private IEnumerator ActivateHitboxAttack()
    {
        StartAnimation();
        offsetAnim = true;
        playerActions.SetIsInAnimation(true);
        yield return new WaitForSeconds(startAttack);
        StartAttacking();
    }

    private IEnumerator DeactivateHitboxAttack()
    {
        yield return new WaitForSeconds(stopAttack);
        offsetAnim = false;
        StopAnimation();
        StopAttacking();
        yield return new WaitForSeconds(0.2f);
        playerActions.SetIsInAnimation(false);
        // attack.SetActive(false);
        gameObject.SetActive(false);
    }

    public void StartAnimation()
    {
        animator.SetTrigger(animationString);
        OffsetAnimation();
    }

    public void OffsetAnimation()
    {
        Vector3 newAnimationOffset = animationOffset;
        if(playerActions.GetIsLookingRight())
            newAnimationOffset.x = newAnimationOffset.x * -1;

        animationPosition.position = transform.position + newAnimationOffset;
        animationFollow.enabled = false;
    }

    public void StopAnimation()
    {
        animator.SetTrigger("Stop");
        animationFollow.enabled = true;
    }

    public void StartAttacking()
    {
        playerActions.SetIsAttacking(true);
        attack.SetActive(true);
    }

    public void StopAttacking()
    {
        playerActions.SetIsAttacking(false);
        attack.SetActive(false);
    }
}
