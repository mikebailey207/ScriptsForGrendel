using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonScript : MonoBehaviour
{
    // Monobehaviour to control the harpoon in Don't Drink and Dive. 
    // Very much a 'it works and it is bug free so on to the next feature' kind of coding, would be neater and better optimized in a studio setting
    public AudioSource shootSound;
    public GameObject heart;
    public Transform player; 
    public LineRenderer lineRenderer;

    private GameObject rock;
    private GameObject[] rock1Objects;

    public float shootSpeed = 20f;
    public float maxDistance = 5f;
    public float retractSpeed = 30f;

    private Vector3 shootDirection;
    private Vector3 startPosition;

    public bool isRetracting = false;
    public bool isAttached;
    private bool retracted = true;
    private bool isStopped = false;
    private bool isShooting = false;


    private int killCount = 0;

    void Start()
    {
        startPosition = transform.localPosition;
        lineRenderer.positionCount = 2; 
        lineRenderer.enabled = false;

        rock1Objects = GameObject.FindGameObjectsWithTag("Rock1");
        Collider2D myCollider = GetComponent<Collider2D>(); 

        foreach (GameObject rock1 in rock1Objects)
        {
            Collider2D groundCollider = rock1.GetComponent<Collider2D>();
            if (groundCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, groundCollider, true);
            }
        }
    }

    void Update()
    {
        transform.rotation = player.transform.rotation;
        lineRenderer.SetPosition(0, player.position);
        lineRenderer.SetPosition(1, transform.position);

        if (Input.GetMouseButtonDown(0) && !isShooting && retracted)
        {
            ShootHarpoon();
        }

        if (isShooting)
        {
            if(!isStopped) transform.position += shootDirection * shootSpeed * Time.deltaTime;

            if (Vector3.Distance(player.position, transform.position) >= maxDistance)
            {
                isStopped = true;               
            }
        }

        if (isAttached && rock != null) // Make sure the BIG BLUE rocks pull back to the player with the harpoon
        {
            rock.transform.position = transform.position;
        }

        if (isRetracting)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, retractSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 0.2f)
            {
                ResetHarpoon();
                if (isAttached) Detach();
            }

            if (Vector3.Distance(transform.position, player.position) < 15f)
            {
               
                if (isAttached) Detach();
            }
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            isStopped = false;
            StartRetracting();
        }
    }

    void Detach()
    {
        rock = null;
        isAttached = false;
    }

    void ShootHarpoon()
    {
        shootSound.Play();
        retracted = false;
        isShooting = true;
        isRetracting = false;
        transform.SetParent(null); 
        lineRenderer.enabled = true;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        shootDirection = (mousePosition - player.position).normalized;
    }

    void StartRetracting()
    {
        isShooting = false;
        isRetracting = true;
    }

    void ResetHarpoon()
    {
        retracted = true;
        isShooting = false;
        isRetracting = false;
        transform.SetParent(player); 
        transform.localPosition = startPosition; 
        lineRenderer.enabled = false;
    }
     
    void DetachFromRock()
    {
        StartRetracting();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("surface"))
        {
            // stop harpoon shooting above the water surface
            StartRetracting();
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
           // retract harpoon when it hits the edges
            StartRetracting();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pike"))
        {
            // Deal damage to the Pike
            if (isShooting)
            {
                StartRetracting();
                GameObject particles = Resources.Load<GameObject>("BloodParticles");
                Instantiate(particles, collision.gameObject.transform.position, Quaternion.identity);
                if (isAttached) Detach();
                collision.gameObject.GetComponent<FishAI>().health--;
              
            }
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // The 'bullet' here is the tiny fish or enemy, a misnomer I know
            if (isShooting)
            {           
                GameObject particles = Resources.Load<GameObject>("BloodParticles");
                Instantiate(particles, collision.gameObject.transform.position, Quaternion.identity);
   
                Destroy(collision.gameObject);
            }
        }
        
        if (collision.gameObject.CompareTag("Rock") && player.GetComponent<FollowMouse2D>().beatPike)
        {
            // Attaches rock to harpoon 
            if (isShooting)
            {
                rock = collision.gameObject;
                isShooting = false;
           
                isAttached = true;
            }
        }
    }
}
