using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    public GameObject bullet;
    public bool fireReady = true;
    private float timer = 1f;
    public Bullet.bulletDirection bulletDirection = Bullet.bulletDirection.xNegative;

    // Use this for initialization
    void Start () {
        bullet.GetComponent<Bullet>().direction = bulletDirection;
    }
	
	// Update is called once per frame
	void Update () {
        if (!fireReady)
            timer -= Time.deltaTime;

        if (timer < 0f)
            fireReady = true;

        if (fireReady){
            Instantiate(bullet, transform.position, Quaternion.identity);
            fireReady = false;
            timer = 1f;
        }
    }
}
