using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public enum bulletDirection { xPositive, xNegative, zPositive, zNegative };

    public bulletDirection direction = bulletDirection.xPositive;
    public float speed = 15f;
    private float timer = 1.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        switch (direction) {
            case bulletDirection.xNegative:
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                break;
            case bulletDirection.xPositive:
                transform.Translate(speed * Time.deltaTime, 0, 0);
                break;
            case bulletDirection.zNegative:
                transform.Translate(0, 0, -speed * Time.deltaTime);
                break;
            case bulletDirection.zPositive:
                transform.Translate(0, 0, speed * Time.deltaTime);
                break;
        }
        timer -= Time.deltaTime;
        if(timer < 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag.Equals("Enemy")) {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

}
