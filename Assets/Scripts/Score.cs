using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    public int points = 10;
    private ScoreController scoreController;

	// Use this for initialization
	void Start () {
		scoreController = GameObject.FindGameObjectWithTag("ScoreController").GetComponent<ScoreController>();     
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals("Player")) {
			scoreController.AddScore(points);
			if (gameObject.tag.Equals("Cup"))
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>().OpenDoor();
			Destroy(gameObject);
		}
    }
}
