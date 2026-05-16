using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FightTrigger : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform camPos;
    [SerializeField] private BoxCollider2D enemyCounter;
    [SerializeField] private List<BoxCollider2D> invisWalls;
    [SerializeField] private Rigidbody2D playerMovement;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInput playerInput;
    private bool cameraOnLocation;

    [SerializeField] private float camSpeed = 2f;

    [SerializeField] private bool bossArena = false;

    AudioSource fightTriggerAudio;
    GameObject mainThemeLoopObject;
    AudioSource mainThemeAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        fightTriggerAudio = GetComponent<AudioSource>();
        mainThemeLoopObject = GameObject.FindGameObjectWithTag("MainThemeLoop");
        mainThemeAudio = mainThemeLoopObject.GetComponent<AudioSource>();
        this.GetComponent<SpriteRenderer>().enabled = false;
        camPos.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        enemyCounter.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        foreach(BoxCollider2D wall in invisWalls)
        {
            wall.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            wall.enabled = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Rigidbody2D>();

        playerController = player.GetComponent<PlayerController>();
        playerInput = player.GetComponent<PlayerInput>();
    }

    void OnDisable()
    {
        mainThemeAudio.Play();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            TriggerFight();
            StartCoroutine("CamOnLoc");
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    } 

    void FixedUpdate()
    {
        if(!cameraOnLocation)
        {
            SetLeftWall();
        }
        if(cameraOnLocation)
        {
            SetInvisibleWalls();
            playerInput.enabled = true;
            playerController.enabled = true;
        }
        if(cameraOnLocation && !mainCamera.GetComponent<ShakeScreen>().shaking)
        {
            mainCamera.transform.position = camPos.position;
        }
           
    }

    private IEnumerator CamOnLoc()
    {
        yield return new WaitForSeconds(camSpeed);
        cameraOnLocation = true;
    }

    private void TriggerFight()
    {
        mainCamera.GetComponent<FollowPlayer>().enabled = false;
        StartCoroutine(camSpeed.Tweeng( (p)=>mainCamera.transform.position=p,
        mainCamera.transform.position,
        camPos.position));

        playerInput.enabled = false;
        playerController.enabled = false;
        playerMovement.linearVelocity = Vector2.zero;

        fightTriggerAudio.Play();
        mainThemeLoopObject.GetComponent<AudioFade>().FadeOut();

        enemyCounter.enabled = true;
        foreach(BoxCollider2D wall in invisWalls)
            wall.enabled = true;
        
        if(bossArena)
            foreach(BoxCollider2D wall in invisWalls)
                wall.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        
        enemyCounter.enabled = false;

    }

    private void SetLeftWall()
    {
        Vector2 leftSide = mainCamera.ViewportToWorldPoint( Vector3.zero );
        invisWalls[0].transform.position = new Vector2(
            leftSide.x - 0.5f,
            mainCamera.transform.position.y
        );
    }

    private void SetInvisibleWalls()
    {
        Vector2 leftSide = mainCamera.ViewportToWorldPoint( Vector3.zero );
        Vector2 rightSide = mainCamera.ViewportToWorldPoint( Vector3.one );
        
        invisWalls[0].transform.position = new Vector2(
            leftSide.x - 0.5f,
            mainCamera.transform.position.y
        );
        invisWalls[1].transform.position = new Vector2(
            rightSide.x + 0.5f,
            mainCamera.transform.position.y
        );
    }
}
