using UnityEngine;
using System.Collections;

public class SinglePlayerController : MonoBehaviour {

    public float runSpeed = 10;

    public float gravity = -12;
    public float jumpHeight = 1;
    public float limitJetPack = 5f;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Animator animator;
    CharacterController controller;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public bool hasPistol = false;
    public bool hasJetPack = false;

    public Vector3 startPoint;

    public GameObject pistolPrefab;
    public GameObject jetPackPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        transform.position = startPoint;
    }

    void OnLevelWasLoaded(int level)
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        transform.position = startPoint;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocityY += Time.deltaTime * gravity;
        Move(input);
        Animating(input);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.F))
            Shoot();

        if (Input.GetMouseButton(1))
            JetPack();
    }

    void Move(Vector2 dir)
    {
        Vector2 inputDir = dir.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = runSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
            velocityY = 0;
    }

    void Animating(Vector2 dir)
    {
        bool walking = dir.x != 0f || dir.y != 0f;
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

    void JetPack()
    {
        if (hasJetPack && transform.position.y < limitJetPack)
        {
            float jumpVelocity = Mathf.Sqrt(-1 * gravity * 2);
            velocityY = jumpVelocity;
        }
    }

    void Shoot()
    {
        if (controller.isGrounded && hasPistol)
        {
            animator.SetTrigger("IsShoot");
            Fire();
        }
    }

    void Fire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;
        Destroy(bullet, 2.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Pistol"))
        {
            hasPistol = true;
            Destroy(other.gameObject);
            pistolPrefab.SetActive(true);
        }
        if (other.gameObject.tag.Equals("JetPack"))
        {
            hasJetPack = true;
            Destroy(other.gameObject);
            jetPackPrefab.SetActive(true);
        }
    }
}
