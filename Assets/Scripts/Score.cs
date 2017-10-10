using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int points = 10;
    private GameController gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();     
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Player")) {
            gameController.AddScore(points);
			if (gameObject.tag.Equals("Cup"))
				GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>().OpenDoor();
			Destroy(gameObject);
		}
    }
}
