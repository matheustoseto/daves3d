using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public bool getCup = false;
    public int newStage = 0;
    AudioSource playerAudio;
    public AudioClip gameOverClip;
    private GameController gameController;

    // Use this for initialization
    void Start () {
        playerAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Player") && getCup)
        {
            if (newStage != 0)
            {
                playerAudio.Play();
                SceneManager.LoadScene("Fase_" + newStage, LoadSceneMode.Single);
                gameController.currentStage++;
            }
            else
            {
                playerAudio.clip = gameOverClip;
                playerAudio.Play();
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }            
    }
}
