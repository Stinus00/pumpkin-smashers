using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCounting : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<BoxCollider2D> invisWalls;
    [SerializeField] private GameObject triggerFight;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 camPos;
    [SerializeField] private Vector3 Offset = new Vector3(0, 3, -20);

    private bool counting = false;
    [SerializeField] private float camSpeed = 2f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("BossEnemy"))
        {
            enemies.Add(other.gameObject);
            counting = true;
        }
    }

    public void DestroyEnemy(GameObject gameObject)
    {
        enemies.Remove(gameObject);
    }

    void FixedUpdate()
    {
        if(enemies.Count == 0 && counting)
        {
            counting = false;
            StartCoroutine("EndFight");
        }
    }

    IEnumerator EndFight()
    {
        triggerFight.GetComponent<AudioFade>().FadeOut();
        yield return new WaitForSeconds(2f);
        mainCamera.GetComponent<FollowPlayer>().enabled = false;
        triggerFight.SetActive(false);

        camPos = new Vector3(player.position.x, Offset.y, Offset.z);

        StartCoroutine(camSpeed.Tweeng( (p)=>mainCamera.transform.position=p,
        mainCamera.transform.position,
        camPos));

        StartCoroutine(EnableEverything(camSpeed));
        foreach(BoxCollider2D wall in invisWalls)
            wall.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator EnableEverything(float duration = 2f)
    {
        yield return new WaitForSeconds(duration);
        mainCamera.GetComponent<FollowPlayer>().enabled = true;
        foreach(BoxCollider2D wall in invisWalls)
            wall.enabled = false;
    }
}
