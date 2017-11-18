using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float runSpeed = 10;

    public float gravity = -12;
    public float jumpHeight = 1;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;    

    public Animator animator;
    CharacterController controller;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public bool hasPistol = false;
    public bool hasJetPack = false;

    public Vector3 startPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        DontDestroyOnLoad(gameObject);
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        transform.position = startPoint;
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
            
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocityY += Time.deltaTime * gravity;
        Move(input);
        Animating(input);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }

        
    }

    void Move(Vector2 dir)
    {
        Vector2 inputDir = dir.normalized;

        //Smooth Rotation
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = runSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
    }

    void Animating(Vector2 dir)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        //bool walking = h != 0f || v != 0f;
        bool walking = dir.x != 0f || dir.y != 0f;

        // Tell the animator whether or not the player is walking.
        animator.SetBool("IsRunning", walking);
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            CmdSetAnimTrigger("IsJumping");
            //animator.SetTrigger("IsJumping");
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }
    
    void Shoot()
    {
        if (controller.isGrounded)
        {
            CmdSetAnimTrigger("IsShoot");
            CmdFire();
        }
    }  

    //Set Trigger Network Animation
    [Command]
    public void CmdSetAnimTrigger(string triggerName)
    {
        if (!isServer)
        {
            animator.SetTrigger(triggerName);
        }
        RpcSetAnimTrigger(triggerName);        
    }

    [ClientRpc]
    public void RpcSetAnimTrigger(string triggerName)
    {
        if(animator != null)
            animator.SetTrigger(triggerName);
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PickUp"))
            CmdSetDestroy(other.gameObject);
    }

    [Command]
    public void CmdSetDestroy(GameObject gameObject)
    {
        if (!isServer)
            Destroy(gameObject);

        RpcSetActive(gameObject);
    }

    [ClientRpc]
    public void RpcSetActive(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
