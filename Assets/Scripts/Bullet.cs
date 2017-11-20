using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.name == "Spider")
        {
            Destroy(collision.transform.root.gameObject);
        }

        if (collision.gameObject.name == "DavePlayer")
        {
            //  Destroy(collision.gameObject);
        }
    }
}
