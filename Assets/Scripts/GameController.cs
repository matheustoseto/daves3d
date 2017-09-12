using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject door;
    public Material materialOpenDoor;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}

    public void OpenDoor() {
        door.GetComponent<MeshRenderer>().material = materialOpenDoor;
        door.GetComponent<LoadScene>().getCup = true;
    }

    public void GetDoor() {
        door = GameObject.FindGameObjectWithTag("Door");
    }
}
