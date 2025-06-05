using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThrowRock : MonoBehaviour
{
    // Script attached to the BIG BLUE rocks in Don't Drink and Dive
    CinemachineImpulseSource cin;
    AudioSource explosionSound, ouchSound;
    GameObject harpoon;

    void Start()
    {
        cin = FindObjectOfType<FollowMouse2D>().GetComponent<CinemachineImpulseSource>();
        explosionSound = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        ouchSound = GameObject.Find("OuchSound").GetComponent<AudioSource>();
        harpoon = FindObjectOfType<HarpoonScript>().gameObject;
    }
 
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Explode through mines, satisfying mechanic
        if (collision.gameObject.CompareTag("Mine") && harpoon.GetComponent<HarpoonScript>().isRetracting && harpoon.GetComponent<HarpoonScript>().isAttached)
        {
            GameObject part = Resources.Load<GameObject>("ExplosionParticles");
            Instantiate(part, transform.position, Quaternion.identity);
            cin.GenerateImpulse();
            explosionSound.Play();
            Destroy(collision.gameObject);
        }
        // Musk is the Loch Ness Monster. Hinting at a previous design idea that I changed :)
        if (collision.gameObject.CompareTag("Musk") && harpoon.GetComponent<HarpoonScript>().isRetracting && harpoon.GetComponent<HarpoonScript>().isAttached)
        {
            GameObject part = Resources.Load<GameObject>("ExplosionParticles");
          
            Instantiate(part, transform.position, Quaternion.identity);
            cin.GenerateImpulse();
            explosionSound.Play();
            ouchSound.Play();
            collision.gameObject.GetComponent<LochNessMonster>().health -= 3;
            harpoon.GetComponent<HarpoonScript>().isAttached = false;
            
        }
    }
}
