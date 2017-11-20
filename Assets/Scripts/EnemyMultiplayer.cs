using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyMultiplayer : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Animator animator;

    public bool fireReady = true;
    private float timer = 1f;


    void Awake()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            animator.SetBool("Atack", true);
            CmdFire();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            animator.SetBool("Atack", false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        if (!fireReady)
            timer -= Time.deltaTime;

        if (timer < 0f)
            fireReady = true;

        if (fireReady)
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 30;

            // Spawn the bullet on the Clients
            NetworkServer.Spawn(bullet);

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }
}
