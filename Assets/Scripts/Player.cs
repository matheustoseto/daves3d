using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private bool jump = true;
    private bool isGround = false;
    public float speed = 3f;
	public bool getPistol = false;
	public GameObject bullet;
    private float timer = 0.5f;
    public bool fireReady = true;

    // Use this for initialization
    void Start () {
        
    }

    private void Awake() {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().GetDoor();
    }

    // Update is called once per frame
    void Update() {
        if (jump)
            speed = 3f;
        else 
            speed = 2f;
        
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.Translate(x, 0, z);

        if (Input.GetKeyDown(KeyCode.Space) && jump && isGround) {           
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 6.5f, 0), ForceMode.Impulse);
            jump = false;
        }

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
}
