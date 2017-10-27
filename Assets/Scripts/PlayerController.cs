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

    Animator animator;
    CharacterController controller;

    private GameObject cameraObj;
    private Vector3 offset = new Vector3(0, 15, -25);

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Awake()
    {
        cameraObj = Camera.main.gameObject;
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
        MoveCamera();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("IsDead");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.SetBool("IsJetPack", true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.SetBool("IsJetPack", false);
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
            animator.SetTrigger("IsJumping");
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    void Shoot()
    {
        if (controller.isGrounded)
        {
            animator.SetTrigger("IsShoot");
        }
        /*else
        {
            return;
        }*/
    }

    void MoveCamera()
    {
        cameraObj.transform.position = transform.position + offset;
    }
}
