using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GroundDie : MonoBehaviour {

    private GameController gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Player"))
        {
            gameController.RemoveLife();
        }
    }
}
