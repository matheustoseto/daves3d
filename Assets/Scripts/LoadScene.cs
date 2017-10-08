using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public bool getCup = false;
    public int newStage = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Player") && getCup)
        {
            if (newStage != 0)
            {
                SceneManager.LoadScene("Fase_" + newStage, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }            
    }
}
