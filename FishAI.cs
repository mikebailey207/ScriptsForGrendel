using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishAI : MonoBehaviour
{
    // Monobehaviour that controls the first fish 'boss' 'McBandito Pike' in 'Don't drink and dive' 
    // Left as it was when week long game jam ended, comments removed and new ones added. 
    // Aware it could be optimized better, magic numbers removed etc.. commented on things that I would do differently. 
    // Very much a 'it works and it is bug free so on to the next feature' kind of coding, would be neater and better optimized in a studio setting

    private AudioSource buildUpSound, dashSound, pikeMusic, afterPikeMusic;
    public GameObject bossDefeatedScreen;
    public GameObject bulletPrefab;

    public AudioSource music1;
    public float health = 10;
    public Transform player;
    public Transform bulletSpawnPoint;

    public Transform shootingPosition; 

    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public float dashSpeed = 8f;
    public float shakeDuration = 3f;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 3f;

    public float shootingDuration = 10f;
    public float bulletInterval = 0.5f;

    private bool hasDetectedPlayer = false;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer gunRenderer;
    private int shakeDashCount = 0;
    public Slider healthSlider;
    private GameObject[] spawnedFish;
    private GameObject mineFish;

    void Start()
    {  // I should really have made these public and assigned them in the inspector, this is not efficient or practical
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
      
        buildUpSound = GameObject.Find("BuildUpSound").GetComponent<AudioSource>();
        dashSound = GameObject.Find("PikeDashSound").GetComponent<AudioSource>();

        pikeMusic = GameObject.Find("PikeMusic").GetComponent<AudioSource>();
        afterPikeMusic = GameObject.Find("AfterPikeMusic").GetComponent<AudioSource>();
    }

    void Update()
    {
        healthSlider.value = health;
        // Following is defeating the pike, would have made more sense to have its own function
        if (health <= 0)
        {
            player.GetComponent<FollowMouse2D>().health = 10;
            player.GetComponent<FollowMouse2D>().oxygen = 50;

            healthSlider.gameObject.SetActive(false);
            pikeMusic.Stop();
            player.GetComponent<FollowMouse2D>().beatPike = true;
            bossDefeatedScreen.SetActive(true);
            Destroy(gameObject);
        }
        
        //Again would have been neater to have a Flip() method
        spriteRenderer.flipX = transform.position.x > player.position.x;
        gunRenderer.flipX = transform.position.x > player.position.x;
        //And a DetectPlayer() method
        if (!hasDetectedPlayer && Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            hasDetectedPlayer = true;
            StopAllCoroutines();
            StartCoroutine(ShakeDashLoop());
        }
    }

    void TurnOffBossDefeatedScreen()
    {
        bossDefeatedScreen.SetActive(false);
    }

    IEnumerator IdleMovement() 
    {
        while (!hasDetectedPlayer)
        {
            float moveTime = Random.Range(.1f, .5f);
            Vector3 moveDirection = Random.value > 0.5f ? Vector3.right : Vector3.left;

            float timer = 0;
            while (timer < moveTime)
            {
                transform.position += moveDirection * moveSpeed/10 * Time.deltaTime;
                spriteRenderer.flipX = moveDirection == Vector3.left;
                gunRenderer.flipX = moveDirection == Vector3.left;
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator ShakeDashLoop() // Where the pike starts attacking the player, and then loops until it's dead
    {
        while (true)
        {
            shakeDashCount = 0;

            while (shakeDashCount < 3)
            {
                yield return StartCoroutine(ShakeThenDash());
       
                shakeDashCount++;
            }

            yield return StartCoroutine(ShootingAttack());
        }
    }

    IEnumerator ShakeThenDash()
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0;
        buildUpSound.Play();
        while (elapsed < shakeDuration)
        {
            //A little shake to warn the player the fish is going to dash at them
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * 0.7f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        buildUpSound.Stop();
        dashSound.Play();
        transform.position = originalPosition;

        Vector3 dashTarget = player.position;
        while (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator ShootingAttack()
    {
        //Moves fish to shooting position where it hovers up and down while releasing tiny fish
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = shootingPosition.position;
        float elapsed = 0f;
        bool firing = true;

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        //Only fire bullets while firing = true - exit coroutine if it's false - tied to System.Func<bool> shouldFire in the FireBullets coroutine
        StartCoroutine(FireBullets(() => firing));


        while (elapsed < shootingDuration)
        {
            Vector3 upPosition = transform.position + Vector3.up * 125f; 
            float moveTime = 0f;
            while (moveTime < 2f) 
            {
                transform.position = Vector2.Lerp(transform.position, upPosition, 1 * Time.deltaTime);
                moveTime += Time.deltaTime;
                yield return null;
            }

            Vector3 downPosition = transform.position + Vector3.down * 125f;
            moveTime = 0f;
            while (moveTime < 2f) 
            {
                transform.position = Vector2.Lerp(transform.position, downPosition, 1 * Time.deltaTime);
                moveTime += Time.deltaTime;
                yield return null;
            }

            elapsed += 4f; 
        }

        firing = false;

        while (Vector2.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FireBullets(System.Func<bool> shouldFire)
    {
        while (shouldFire())
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            
            yield return new WaitForSeconds(bulletInterval);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Rock"))
        {
            health--;        
        }
    }
}
