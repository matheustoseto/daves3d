using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed = 15f;
    private float timer = 1.5f;
    private GameController gameController;
    public string targetTag = "";

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if(timer < 0f)
            Destroy(gameObject);
    }

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag.Equals(targetTag))
        {
            if ("Player".Equals(targetTag))
            {
                gameController.RemoveLife();
            }
            else
            {
                Destroy(collision.gameObject);
            }         
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
    */
}
