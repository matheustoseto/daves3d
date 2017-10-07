using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

    private string fmt = "00000";
    public int score = 0;
    public Font font;

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
        GUI.skin.font = font;
        GUI.skin.label.fontSize = 40;

        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.Label(new Rect(30, 30, 300, 50), score.ToString(fmt));
        GUI.EndGroup();
        
    }
	
	public void AddScore(int scr) {
        score += scr;
    }
}
