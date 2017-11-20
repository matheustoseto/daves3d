using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            other.gameObject.GetComponent<MultiGameController>().RemoveLife();
    }
}
