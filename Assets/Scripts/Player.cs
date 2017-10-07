﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private bool jump = true;
    private bool isGround = false;
	public bool getPistol = false;
	public GameObject bullet;
    private float timer = 0.5f;
    public bool fireReady = true;

    public float speed = 6f;            // The speed that the player will move at.
    Vector3 movement;                   // The vector to store the direction of the player's movement.
    //Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 50f;          // The length of the ray from the camera into the scene.


    // Use this for initialization
    void Start () {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Awake() {
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().GetDoor();
    }

    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(h, v);

        // Turn the player to face the mouse cursor.
        Turning();

        // Jump Player
        Jump();
    }

    // Update is called once per frame
    void Update() {

        getDirectionBullet();
        if (Input.GetKeyDown(KeyCode.LeftControl) && getPistol && fireReady) {
            Instantiate(bullet, transform.position, Quaternion.identity);
            fireReady = false;
            timer = 0.5f;
        }
        
        if(!fireReady)
            timer -= Time.deltaTime;

        if (timer < 0f) 
            fireReady = true;
        
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    private void Jump()
    {
        if (jump)
            speed = 3f;
        else
            speed = 2f;

        if (Input.GetKeyDown(KeyCode.Space) && jump && isGround)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 6.5f, 0), ForceMode.Impulse);
            jump = false;
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag.Equals("Ground")) {
            isGround = true;
        }else {
            isGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Ground")) {
            jump = true;
        }
		if (collision.gameObject.tag.Equals("Pistol")) {
            getPistol = true;
            Destroy(collision.gameObject);
        }
    }

    private void getDirectionBullet(){
        if (Input.GetKeyDown(KeyCode.UpArrow))
            bullet.GetComponent<Bullet>().direction = Bullet.bulletDirection.zPositive;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            bullet.GetComponent<Bullet>().direction = Bullet.bulletDirection.zNegative;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            bullet.GetComponent<Bullet>().direction = Bullet.bulletDirection.xPositive;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            bullet.GetComponent<Bullet>().direction = Bullet.bulletDirection.xNegative;
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
        }
    }
}
