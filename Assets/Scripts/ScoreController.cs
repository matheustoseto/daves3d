using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

    private string fmt = "00000";
    public int score = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 300, 50), "Score: " + score.ToString(fmt));
    }
	
	public void AddScore(int scr) {
        score += scr;
    }
}
