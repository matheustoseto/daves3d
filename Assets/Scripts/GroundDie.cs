using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GroundDie : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
